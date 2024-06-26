using Front.Proxys;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers;

public class AdminController : Controller
{
    private readonly Proxy _proxy;

    public AdminController(Proxy proxy)
    {
        _proxy = proxy;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string actionType, string _ReferralCode)
    {
        if (HasAdminAccess())
        {
            switch (actionType)
            {
                case "MakeClientBusiness":
                    // Handle Make Client Business action
                    return RedirectToAction("MakeClientBusiness", new { referralCode = _ReferralCode });

                case "MakeClientAdmin":
                    // Handle Make Client Admin action
                    return RedirectToAction("MakeClientAdmin", new { referralCode = _ReferralCode });

                case "DeleteClient":
                    // Handle Delete Client action
                    return RedirectToAction("DeleteClient", new { referralCode = _ReferralCode });

                case "GetClient":
                    // Handle Get Client action
                    return RedirectToAction("GetClient", new { referralCode = _ReferralCode });

                case "GetAllClients":
                    // Handle Get All Clients action
                    return RedirectToAction("GetAllClients");
                
                case "DownloadGetAll":
                    //Handle Download excel of all clients
                    return RedirectToAction("DownloadGetAll");

                default:
                    // Handle unknown action
                    return BadRequest();
            }
        }
        return Unauthorized("You are not authorized to use this feature");
    }
    
    public async Task<IActionResult> MakeClientBusiness(string referralCode)
    {
        var response = await _proxy.MakeClientBusinessProxy(referralCode);
        if (response != null)
        {
            return Redirect(response);
        }
        return View("FailedOperation");
    }

    public async Task<IActionResult> MakeClientAdmin(string referralCode)
    {
        var response = await _proxy.MakeClientAdminProxy(referralCode);
        if (response != null)
        {
            return Redirect(response);
        }
        return View("FailedOperation");
    }

    public IActionResult DeleteClient(string referralCode)
    {
        var response = _proxy.DeleteClientProxy(referralCode);
        if (response.Result.IsSuccessStatusCode)
        {
            return View("SuccessfulOperation");
        }
        return View("FailedOperation");
    }

    public IActionResult GetClient(string referralCode)
    {
        var response = _proxy.GetClientProxy(referralCode);
        if (response.Result.IsSuccessStatusCode)
        {
            return View("SuccessfulOperation");
        }
        return View("FailedOperation");
    }

    public IActionResult GetAllClients()
    {
        var response = _proxy.GetAllClientsProxy();
        if (response.Result.IsSuccessStatusCode)
        {
            return View("SuccessfulOperation");
        }
        return View("FailedOperation");
    }

    private bool HasAdminAccess()
    {
        var id = HttpContext.Session.GetString("ClientId");
        var response = _proxy.HasAdminAccessProxy(id);
        if (response.Result.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }

    public async Task<IActionResult> DownloadGetAll()
    {
        HttpResponseMessage response = await _proxy.DownloadGetAllProxy();

        Stream responseStream = await response.Content.ReadAsStreamAsync();
        return File(responseStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "clients.xlsx");
    }
}