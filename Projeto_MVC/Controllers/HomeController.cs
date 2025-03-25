using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_MVC.Models;
using Microsoft.EntityFrameworkCore;
using Projeto_MVC.Data;
using SQLitePCL;

namespace Projeto_MVC.Controllers;

public class HomeController : Controller
{
   
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;

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
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        // Consulta no banco para verificar email e senha
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.username && u.Password == model.password);

        if (user == null)
        {
            ModelState.AddModelError("", "Usu�rio ou senha inv�lidos");
            return View("Index");
        }

        // Caso encontre o usu�rio, autentica
        var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
        var identity = new ClaimsIdentity(claims, "BasicAuthentication");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Services");
    }

    [Authorize]
    public async Task<IActionResult> Services()
    {
        // Obt�m o username (email) do usu�rio a partir das Claims
        var username = User.Identity?.Name;

        // Consulta o banco de dados para obter o usu�rio completo
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user != null)
        {
            // Passa o FullName para a View atrav�s do ViewBag
            ViewBag.FullName = user.FullName;
        }
        else
        {
            // Caso o usu�rio n�o seja encontrado, redireciona ou trata o erro
            return RedirectToAction("Error");
        }

        return View();
    }


    [Authorize]
    public async Task<IActionResult> Profile()
    {
        // Recupera o nome de usu�rio (email) das Claims
        var username = User.Identity?.Name;

        // Busca o usu�rio no banco de dados
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user != null)
        {
            ViewBag.FullName = user.FullName;
            ViewBag.Email = user.UserEmail;
        }
        else
        {
            // Caso o usu�rio n�o seja encontrado, trata o redirecionamento
            return RedirectToAction("Error");
        }

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
        // Cria o novo usu�rio
        var newUser = new UserModel
        {
            FullName = fullName,
            CPF = cpf,
            BirthDate = birthDate,
            Username = username,
            Password = password,
            UserEmail = username
        };

        // Adiciona o usu�rio ao banco de dados
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync(); // Salva as altera��es no banco
        _context.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL;");
        _logger.LogInformation("Mudan�as salvas no banco de dados com sucesso.");


        // Autenticar o novo usu�rio
        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, "BasicAuthentication");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Services");
    }


    public IActionResult IncomeTaxCalculation()
    {
        return View();
    }

    [HttpPost]
    public IActionResult IncomeTaxCalculation(IncomeTaxModel model)
    {
        if (ModelState.IsValid)
        {
            var imposto = model.CalcularImposto(); // Chamando o m�todo diretamente na Model
            ViewBag.Imposto = imposto; // Envia o imposto calculado para a View
            Debug.WriteLine($"Imposto Calculado: {imposto}");
            return View(model); // Retorna a mesma View com o resultado
        }
        return View(model); // Retorna a View com os erros de valida��o, se houver
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}