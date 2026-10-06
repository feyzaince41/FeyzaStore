using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace FeyzaStore.MVC.Controllers;

public class OrderController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OrderController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();

        var token = User.FindFirst("AccessToken")?.Value;

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var orders = await client.GetFromJsonAsync<List<OrderViewModel>>(
            "https://localhost:7108/api/Order"
        );

        return View(orders ?? new List<OrderViewModel>());
    }
}