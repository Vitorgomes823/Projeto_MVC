using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Projeto_MVC.Models;

namespace Projeto_MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        HomeModel home = new HomeModel();
        home.UserName = "adimin";
        home.UserEmail = "adm.vh@fi-group.com";
        return View(home);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Services()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult IncomeTaxCalculation()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
