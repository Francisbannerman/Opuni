using System.Text;
using Microsoft.IdentityModel.Tokens;
using Referral.Dtos.ReferralDto;
using Referral.Model;

namespace Referral;

public static class Extensions
{
    public static ClientDto AsDto(this Client client)
    {
        return new ClientDto
        {
            Id = client.Id, FirstName = client.FirstName, LastName = client.LastName,
            TelephoneNumber = client.TelephoneNumber, 
            ReferralCode = client.ReferralCode, DateCreated = client.DateCreated, 
            CreatedUsingReferralCode = client.CreatedUsingReferralCode,
            NumberOfTimeReferralHasBeenUsed = client.NumberOfTimeReferralHasBeenUsed,
            Role = client.Role
        };
    }
}

public static class SD
{
    public const string Role_Client = "Client";
    public const string Role_Admin = "Admin";
}