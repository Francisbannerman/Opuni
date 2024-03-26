using System.Net;
using Microsoft.AspNetCore.Mvc;
using Referral.Dtos.ReferralDto;
using Referral.Model;
using Referral.Repositories;
using Referral.Services;
using System.Security.Claims;
using Front.Model;
using Referral.EndPoints;

namespace Referral.Controllers;

[ApiController]
[Route("api/referral")]
public class ReferralController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IEndPoints _endPoints;
    private readonly IUnitOfWork _unitOfWork;

    public ReferralController(IUnitOfWork unitOfWork,
        IEndPoints endPoints, IPaymentService paymentService)
    {
        _unitOfWork = unitOfWork;
        _endPoints = endPoints;
        _paymentService = paymentService;
    }
    
    [HttpGet("{id}")]
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
    
    [HttpGet("getall")]
    public ActionResult<IEnumerable<ClientDto>> GetAllClients()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var referrer = _unitOfWork.Client.GetSpecial(u => u.Id.ToString() == userId);

        if (referrer.Role != SD.Role_Admin)
        {
            return Unauthorized();
        }
        var clients = _unitOfWork.Client.GetAll().Select(client => client.AsDto());
        if (clients == null)
        {
            return NotFound();
        }
        return clients.ToList();
    }

    [HttpPost("create")]
    public ActionResult<ClientDto> CreateClient(CreateClientDto clientDto)
    {
        var createdClient = _endPoints.CreateClientEndPoint(clientDto);
        return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
    }

    [HttpDelete("{id}")]
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

    [HttpPut("admin/{id}")]
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
    
    [HttpPut("business/{id}")]
    public ActionResult MakeClientBusiness(Guid id)
    {
        var existingClient = _unitOfWork.Client.Get(id);
        if (existingClient == null)
        {
            return NotFound();
        }
        var stripeAccNum = _paymentService.CreateConnectedAccount();
        existingClient.IsBusiness = true;
        existingClient.StripeAccountId = stripeAccNum;
        existingClient.StripeAccountLink = _paymentService.GenerateAccountLink(stripeAccNum);
        existingClient.Role = SD.Role_Business;
        _unitOfWork.Client.Edit(existingClient);
        _unitOfWork.Save();
        return NoContent();
    }

    [HttpPost("makepayment")]
    public IActionResult MakePayment(string AmountPaid, string ClientId)
    {
        var client = _unitOfWork.Client.GetSpecial(u => u.Id == Guid.Parse(ClientId));
        var clientName = $"{client.FirstName} {client.LastName}";
        var sessionUrl = _paymentService.StripePayment(Convert.ToDecimal(AmountPaid), clientName, client.Id);
        return Content(sessionUrl);
    }
    
}