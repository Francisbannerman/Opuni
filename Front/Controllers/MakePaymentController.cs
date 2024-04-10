using Front.Models;
using Front.Proxys;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers;

public class MakePaymentController : Controller
{
    private readonly Proxy _proxy;
    public MakePaymentController(Proxy proxy)
    {
        _proxy = proxy;
    }
    
    // GET
    public IActionResult Index()
    {
        
        return View();
    }

    [HttpPost]
    public async Task <IActionResult> Index(AmountPaid amountPaid)
    {
        amountPaid._ClientId = HttpContext.Session.GetString("ClientId");
        if (ModelState.IsValid)
        {
            var response = await _proxy.MakePaymentProxy(amountPaid);
            if (response != null)
            {
                return Redirect(response);
            }
            else
            {
                ModelState.AddModelError(string.Empty, 
                    "Failed to create client. Please try again later.");
            }
        }
        return View("FailedPayment");
    }

    public IActionResult FailedPayment()
    {
        return View();
    }

    public IActionResult SuccessfulPayment()
    {
        return View();
    }
}