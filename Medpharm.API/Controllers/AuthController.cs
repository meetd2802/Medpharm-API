using Microsoft.AspNetCore.Mvc;

namespace Medpharm.API.Controllers;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}