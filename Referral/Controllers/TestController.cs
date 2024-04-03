using Microsoft.AspNetCore.Mvc;
using Referral.Services;
using Stripe;

namespace Referral.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : Controller
{
    private readonly IConfiguration _config;
    private IPaymentService _paymentService;
    public TestController(IPaymentService paymentService,
        IConfiguration config)
    {
        _paymentService = paymentService;
        _config = config;
    }

    [HttpPost]
    public IActionResult TestCreateStripe(Guid id)
    {

        return Content(_paymentService.StripePayment(id, 
            _paymentService.CreatePrice(5),
            _paymentService.CreateConnectedAccount()));
    }
}