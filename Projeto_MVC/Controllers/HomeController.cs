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
        new UserModel { FullName = "User Teste1", CPF = "12345678900", BirthDate = new DateTime(2000, 1, 1), Username = "user1@teste.com", Password = "SenhaTeste1" },
        new UserModel { FullName = "User Teste2", CPF = "09876543211", BirthDate = new DateTime(2001, 2, 2), Username = "user2@teste.com", Password = "SenhaTeste2" }
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


    //Trabalhando nesse
    //[HttpPost]
    //public IActionResult Update(UserModel updatedUser)
    //{
    //    var user = Users.SingleOrDefault(u => u.Username == updatedUser.Username);
    //    if (user == null)
    //    {
    //        return NotFound();
    //    }

    //    user.FullName = updatedUser.FullName;
    //    user.CPF = updatedUser.CPF;
    //    user.BirthDate = updatedUser.BirthDate;
    //    user.Password = updatedUser.Password;

    //    return RedirectToAction("Index");
    //}



    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = Users.SingleOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password"); //descobrir pq erro não exibe
            return View("Index");
        }

        var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
        var identity = new ClaimsIdentity(claims, "BasicAuthentication");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Services");
    }
    [Authorize]
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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> NewSignUp(string fullName, string cpf, DateTime birthDate, string username, string password)
    {
        // Adicione o novo usuário lista de usuários
        Users.Add(new UserModel { FullName = fullName, CPF = cpf, BirthDate = birthDate, Username = username, Password = password });

        //// Autenticar o novo usuário
        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, "BasicAuthentication");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index"); 
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