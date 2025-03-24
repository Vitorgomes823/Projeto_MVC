using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Projeto_MVC.Models;
using Projeto_MVC.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=CreateAcount.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add cookie auth service
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
{
    Options.LoginPath = "/Home/Login";
}).AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private static readonly List<UserModel> Users = new List<UserModel>
    {
        new UserModel {Username = "UserTeste1", Password = "SenhaTeste1"},
        new UserModel {Username = "UserTeste2", Password = "SenhaTeste2"}
    };

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        }
        var authHeader = Request.Headers["Authorization"].ToString();
        var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);
        if (authHeaderVal.Scheme != "Basic")
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Scheme"));
        }
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderVal.Parameter)).Split(':');
        var username = credentials[0];
        var password = credentials[1];

        var user = Users.SingleOrDefault(u => u.Username == username && u.Password == password);

        if (user == null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Username Or Password"));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
