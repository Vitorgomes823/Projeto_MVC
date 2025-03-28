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
using Microsoft.AspNetCore.Http;

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
    public IActionResult Privacy()
    {
        ViewBag.MostrarBotao = true;
        return View();
    }


    //Trabalhando nesse
    [HttpPost]
    public IActionResult Update(UserModel updatedUser)
    {
        var user = Users.SingleOrDefault(u => u.Username == updatedUser.Username);
        if (user == null)
        {
            return NotFound();
        }

        user.FullName = updatedUser.FullName;
        user.CPF = updatedUser.CPF;
        user.BirthDate = updatedUser.BirthDate;
        user.Password = updatedUser.Password;

        return RedirectToAction("Profile");
    }

    public IActionResult Index()
    {
        ViewBag.MostrarBotao = false;
        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index"); // Retorna a p�gina de login se o modelo for inv�lido.
        }

        // Consulta no banco para verificar email e senha
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.username && u.Password == model.password);

        if (user == null)
        {
            ModelState.AddModelError("", "Usu�rio ou senha inv�lidos");
            return View("Index"); // Retorna a p�gina de login se o usu�rio n�o for encontrado.
        }

        // Caso encontre o usu�rio, autentica
        var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
        var identity = new ClaimsIdentity(claims, "BasicAuthentication");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        model.rememberMe = false;  // Garante que o checkbox est� desmarcado

        // Verifica se "Lembre de mim" est� marcado
        if (model.rememberMe)
        {
            View("Services");
        }
        else
        {
            Response.Cookies.Delete("Username"); // Remove o cookie se "Lembre de mim" n�o estiver marcado
        }

        // Retorna ao painel ou p�gina de servi�os ap�s autentica��o bem-sucedida
        return RedirectToAction("Services");
    }


    [Authorize]
    public async Task<IActionResult> Services()
    {
        ViewBag.MostrarBotao = true;

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
        ViewBag.MostrarBotao = true;
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


    public static string FormatarCPF(string CPF)
    {
        if (string.IsNullOrEmpty(CPF) || CPF.Length != 11)
            return CPF; // Retorna o CPF sem altera��es se ele for inv�lido

        // Formata o CPF manualmente
        return $"{CPF.Substring(0, 3)}.{CPF.Substring(3, 3)}.{CPF.Substring(6, 3)}-{CPF.Substring(9, 2)}";
    }

    [Authorize]
    public async Task<IActionResult> Edit()
    {
        ViewBag.MostrarBotao = true;
        var username = User.Identity?.Name;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user != null)
        {
            ViewBag.FullName = user.FullName;
            ViewBag.CPF = FormatarCPF(user.CPF);
            ViewBag.Birthdate = user.BirthDate;
            ViewBag.Email = user.UserEmail;
            ViewBag.Password = user.Password;
        }
        else
        {
            // Caso o usu�rio n�o seja encontrado, trata o redirecionamento
            return RedirectToAction("Error");
        }
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
        // In�cio da l�gica de cria��o de usu�rio
        try
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

            // Adiciona o novo usu�rio ao banco de dados
            _context.Users.Add(newUser);

            // Salva as altera��es no banco
            await _context.SaveChangesAsync();
            _context.Database.ExecuteSqlRaw("PRAGMA wal_checkpoint(FULL);");
            _logger.LogInformation("Usu�rio salvo com sucesso no banco de dados.");

            // Autentica��o do usu�rio ap�s cadastro
            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Services");
        }
        catch (Exception ex)
        {
            // Loga o erro para an�lise e debugging
            _logger.LogError($"Erro ao salvar no banco de dados: {ex.Message}");

            // Redireciona para uma p�gina de erro ou retorna uma mensagem de erro
            ModelState.AddModelError("", "Erro ao criar a conta. Tente novamente.");
            return View("SignUp");
        }
    }



    public IActionResult IncomeTaxCalculation()
    {
        ViewBag.MostrarBotao = true;
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