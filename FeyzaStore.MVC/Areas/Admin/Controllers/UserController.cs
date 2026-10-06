using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace FeyzaStore.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient();

        var token = User.FindFirstValue("AccessToken");

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
        }

        return client;
    }

    public async Task<IActionResult> Index()
    {
        var client = CreateClient();

        var users =
            await client.GetFromJsonAsync<List<AdminUserViewModel>>(
                "https://localhost:7108/api/AdminUser"
            );

        return View(users ?? new List<AdminUserViewModel>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MakeAdmin(string id)
    {
        var client = CreateClient();

        await client.PostAsync(
            $"https://localhost:7108/api/AdminUser/{id}/make-admin",
            null
        );

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveAdmin(string id)
    {
        var client = CreateClient();

        await client.PostAsync(
            $"https://localhost:7108/api/AdminUser/{id}/remove-admin",
            null
        );

        return RedirectToAction(nameof(Index));
    }
}