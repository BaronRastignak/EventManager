using Microsoft.AspNetCore.Mvc;

namespace EventManagerService.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
}
