using System.Net;
using Microsoft.AspNetCore.Mvc;
using Referral.Dtos.ReferralDto;
using Referral.Model;
using Referral.Repositories;
using Referral.Services;
using Referral.EndPoints;

namespace Referral.Controllers;

[ApiController]
[Route("api/referral")]
public class ReferralController : ControllerBase
{
    private readonly IEndPoints _endPoints;

    public ReferralController(IEndPoints endPoints)
    {
        _endPoints = endPoints;
    }
    
    [HttpGet("{referralCode}")]
    public ActionResult<ClientDto> GetClient(string referralCode)
    {
        return _endPoints.GetClientEndPoint(referralCode);
    }
    
    [HttpGet("getall")]
    public ActionResult<IEnumerable<ClientDto>> GetAllClients()
    {
        return Ok(_endPoints.GetAllClientsEndPoint());
    }

    [HttpPost("create")]
    public ActionResult<ClientDto> CreateClient(CreateClientDto clientDto)
    {
        var createdClient = _endPoints.CreateClientEndPoint(clientDto);
        return Ok(createdClient.Id);
    }

    [HttpDelete("delete/{referralCode}")]
    public ActionResult DeleteClient(string referralCode)
    {
        _endPoints.DeleteClientEndPoint(referralCode);
        return NoContent();
    }

    [HttpPut("admin/{referralCode}")]
    public ActionResult MakeClientAdmin(string referralCode)
    {
        var client = _endPoints.MakeClientAdminEndPoint(referralCode);
        return Ok(client);
    }
    
    [HttpPut("business/{referralCode}")]
    public ActionResult MakeClientBusiness(string referralCode)
    {
        var client = _endPoints.MakeClientBusinessEndPoint(referralCode);
        return Ok(client);
    }

    [HttpPost("makepayment")]
    public IActionResult MakePayment(AmountPaid amountPaid)
    {
        return Ok(_endPoints.MakePaymentEndPoint(amountPaid));
    }

    [HttpGet("hasadminaccess/{id}")]
    public IActionResult HasAdminAccess(string id)
    {
        if (_endPoints.HasAdminAccess(id))
        {
            return Ok();
        }
        return Unauthorized();
    }

    [HttpGet("downloadreport")]
    public IActionResult DownloadReport()
    {
        var file = _endPoints.DownloadReport();
        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "clients.xlsx");
    }
    
}