using Microsoft.AspNetCore.Mvc;
using Referral.Dtos.ReferralDto;
using Referral.Model;
using Referral.Repositories;
using Referral.Services;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Referral.Settings;

namespace Referral.Controllers;

[ApiController]
[Route("api/referral")]
public class ReferralController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ReferralCodeService _referralCodeService;
    private readonly AuthorizationSettings _authorizationSettings;

    public ReferralController(IUnitOfWork unitOfWork, ReferralCodeService referralCodeService,
        AuthorizationSettings authorizationSettings)
    {
        _unitOfWork = unitOfWork;
        _referralCodeService = referralCodeService;
        _authorizationSettings = authorizationSettings;
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<ClientDto> GetClient(Guid id)
    {
        string accessToken = HttpContext.Request.Headers["Authorization"];
        var client = _unitOfWork.Client.Get(id);
        if (client == null)
        {
            return NotFound();
        }
        return client.AsDto();
    }
    
    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<ClientDto>> GetAllClients()
    {
        var clients = _unitOfWork.Client.GetAll().Select(client => client.AsDto());
        if (clients == null)
        {
            return NotFound();
        }
        return clients.ToList();
    }

    [HttpPost]
    public ActionResult<ClientDto> CreateClient(CreateClientDto clientDto)
    {
        if (!_unitOfWork.Client.Exists(r => r.ReferralCode == clientDto.CreatedUsingReferralCode))
        {
            return BadRequest("You used a non-existent referral code. Re-look at the referral code " +
                              "and try again with the appropriate referral code");
        }
        _authorizationSettings.IsAdminValid(clientDto);
        _authorizationSettings.GenerateJwtToken(clientDto);

        var id = Guid.NewGuid();
        Client client = new()
        {
            Id = id, FirstName = clientDto.FirstName, LastName = clientDto.FirstName,
            TelephoneNumber = clientDto.TelephoneNumber, 
            ReferralCode = _referralCodeService.GenerateUserReferralCode(clientDto.FirstName, clientDto.LastName, id),
            DateCreated = DateTimeOffset.Now, CreatedUsingReferralCode = clientDto.CreatedUsingReferralCode,
            NumberOfTimeReferralHasBeenUsed = 0, Role = SD.Role_Client
        };
        _unitOfWork.Client.Add(client);
        _referralCodeService.IncreaseReferrersCountOfReferree(clientDto.CreatedUsingReferralCode);
        _unitOfWork.Save();
        
        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client.AsDto());
    }

    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult DeleteClient(Guid id)
    {
        var existingClient = _unitOfWork.Client.Get(id);
        if (existingClient == null)
        {
            return NotFound();
        }
        _unitOfWork.Client.Remove(existingClient);
        _unitOfWork.Save();
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult MakeClientAdmin(Guid id)
    {
        var existingClient = _unitOfWork.Client.Get(id);
        if (existingClient == null)
        {
            return NotFound();
        }
        existingClient.Role = SD.Role_Admin;
        _unitOfWork.Client.Edit(existingClient);
        _unitOfWork.Save();
        return NoContent();
    }
    
    [HttpPost("authenticate")]
    public IActionResult Authenticate(CreateClientDto clientDto)
    {
        if (_authorizationSettings.IsAdminValid(clientDto))
        {
            var token = _authorizationSettings.GenerateJwtToken(clientDto);
            return Ok(new { token });
        }
        else
        {
            return Unauthorized();
        }
    }
}