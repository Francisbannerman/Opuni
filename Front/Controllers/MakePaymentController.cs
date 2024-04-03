using System.Net;
using System.Security.Claims;
using Front.Models;
using Front.Proxys;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Referral.Repositories;

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
        var clientId = HttpContext.Session.GetString("ClientId");
        if (ModelState.IsValid)
        {
            var response = await _proxy.MakePaymentProxy(amountPaid._AmountPaid, clientId);

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

    public IActionResult SuccessfulPayment()
    {
        return View();
    }

    public IActionResult FailedPayment()
    {
        return View();
    }
}