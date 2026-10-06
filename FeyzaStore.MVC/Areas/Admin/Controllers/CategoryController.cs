using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FeyzaStore.MVC.Models;
using System.Net.Http.Json;

namespace FeyzaStore.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();

        var categories = await client.GetFromJsonAsync<List<CategoryViewModel>>(
            "https://localhost:7108/api/Category"
        );

        return View(categories ?? new List<CategoryViewModel>());
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        var token = User.FindFirst("AccessToken")?.Value;

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token
            );

        var response = await client.PostAsJsonAsync(
            "https://localhost:7108/api/Category",
            model
        );

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();

        var token = User.FindFirst("AccessToken")?.Value;

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token
            );

        var response = await client.DeleteAsync(
            $"https://localhost:7108/api/Category/{id}"
        );

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient();

        var category = await client.GetFromJsonAsync<CategoryViewModel>(
            $"https://localhost:7108/api/Category/{id}"
        );

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryViewModel model)


    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();

        var token = User.FindFirst("AccessToken")?.Value;

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token
            );

        var response = await client.PutAsJsonAsync(
            $"https://localhost:7108/api/Category/{model.Id}",
            model
        );

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }
}