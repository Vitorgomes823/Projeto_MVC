using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    private static readonly List<UserModel> Users = new List<UserModel>
        {
            new UserModel { Username = "User@Teste1", Password = "SenhaTeste1" },
            new UserModel { Username = "User@Teste2", Password = "SenhaTeste2" }
        };

    [AllowAnonymous]
    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return View("Index");
        }

        HomeModel home = new HomeModel();
        home.UserName = "adimin";
        home.UserEmail = "adm.vh@fi-group.com";
        return View(home);
    }
    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = Users.SingleOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View("Index");
        }

        var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
        var identity = new ClaimsIdentity(claims, "BasicAuthentication");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Services");
    }

    public IActionResult Services()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult Edit()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult SignUp()
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
