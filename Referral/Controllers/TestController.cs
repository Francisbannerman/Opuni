using Microsoft.AspNetCore.Mvc;
using Referral.EndPoints;
using Referral.Model;
using Referral.Services;
using Stripe;

namespace Referral.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : Controller
{
    private readonly IConfiguration _config;
    private IPaymentService _paymentService;
    private IEndPoints _endPoints;
    public TestController(IPaymentService paymentService,
        IConfiguration config, IEndPoints endPoints)
    {
        _paymentService = paymentService;
        _config = config;
        _endPoints = endPoints;
    }

    [HttpPost]
    public IActionResult TestCreateStripe(AmountPaid amountPaid)
    {

        return Content(_endPoints.MakePaymentEndPoint(amountPaid));
    }
}