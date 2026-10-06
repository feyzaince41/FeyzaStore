using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Security.Claims;

namespace FeyzaStore.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // KAYIT SAYFASI
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // KAYIT İŞLEMİ
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "https://localhost:7108/api/Auth/register",
            model
        );

        if (!response.IsSuccessStatusCode)
        {
            // API'den gelen gerçek hata mesajını alıyoruz.
            var errorMessage =
                await response.Content.ReadAsStringAsync();

            ViewBag.Error = errorMessage;

            return View(model);
        }

        return RedirectToAction("Login");
    }

    // GİRİŞ SAYFASI
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // GİRİŞ İŞLEMİ
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "https://localhost:7108/api/Auth/login",
            model
        );

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "E-posta veya şifre hatalı.";

            return View(model);
        }

        var loginResponse =
            await response.Content
                .ReadFromJsonAsync<LoginResponseViewModel>();

        if (loginResponse == null)
        {
            ViewBag.Error =
                "Giriş işlemi sırasında bir hata oluştu.";

            return View(model);
        }

        // Kullanıcının bilgilerini Cookie içine koyuyoruz.
        var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                loginResponse.UserId
            ),

            new Claim(
                ClaimTypes.Email,
                loginResponse.Email
            ),

            new Claim(
                "AccessToken",
                loginResponse.Token
            )
        };

        // Kullanıcının rollerini ekliyoruz.
        foreach (var role in loginResponse.Roles)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    role
                )
            );
        }

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        var principal =
            new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal
        );

        return RedirectToAction(
            "Index",
            "Home"
        );
    }

    // ÇIKIŞ
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return RedirectToAction(
            "Index",
            "Home"
        );
    }
}