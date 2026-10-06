using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FeyzaStore.MVC.Controllers;

public class AddressController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AddressController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddressViewModel model)
    {
        var client = _httpClientFactory.CreateClient();

        var token = User.FindFirst("AccessToken")?.Value;

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync(
            "https://localhost:7108/api/Address",
            model
        );

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index", "Checkout");
    }
}