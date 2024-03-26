using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Referral.Dtos.ReferralDto;
using Referral.Repositories;
using Microsoft.Extensions.Configuration;

namespace Referral.Settings;

public class AuthorizationSettings
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public AuthorizationSettings(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }
    
    public bool IsAdminValid(CreateClientDto clientDto)
    {
        var referrer = _unitOfWork.Client.GetSpecial
            (u => u.ReferralCode == clientDto.CreatedUsingReferralCode);
        
        if (referrer.Role == "Admin" && clientDto.FirstName != null && 
            clientDto.LastName != null && clientDto.TelephoneNumber.ToString() != null)
        {
            return true;
        }
        return false;
    }

    public string GenerateJwtToken(CreateClientDto clientDto)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, $"{clientDto.FirstName} {clientDto.LastName}"),
            new Claim(ClaimTypes.MobilePhone, clientDto.TelephoneNumber),
            new Claim(ClaimTypes.SerialNumber, clientDto.CreatedUsingReferralCode)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Auth0:ClientSecret"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}