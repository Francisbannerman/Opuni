using Microsoft.AspNetCore.Mvc;
using Front.Proxys;
using Referral.Dtos.ReferralDto;

namespace Front.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Proxy _proxy;
    
    public HomeController(ILogger<HomeController> logger, Proxy proxy)
    {
        _logger = logger;
        _proxy = proxy;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task <IActionResult> Index(CreateClientDto clientDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _proxy.CreateClientProxy(clientDto);
            if (response.Response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("ClientId", response.CreatedClientId.ToString());
                return RedirectToAction("Index", "MakePayment");
            }
            ModelState.AddModelError(string.Empty, 
                "Failed to create client. Please try again later.");
        }
        return View();
    }

    
}