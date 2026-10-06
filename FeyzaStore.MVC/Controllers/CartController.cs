using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FeyzaStore.MVC.Controllers;

public class CartController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CartController(IHttpClientFactory httpClientFactory)
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

        var response = await client.GetAsync(
            "https://localhost:7108/api/Cart"
        );

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return View(new CartViewModel());
        }

        response.EnsureSuccessStatusCode();

        var cart = await response.Content.ReadFromJsonAsync<CartViewModel>();

        return View(cart ?? new CartViewModel());
    }


    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
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
            "https://localhost:7108/api/Cart/add",
            new
            {
                ProductId = productId,
                Quantity = quantity
            });

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        var client = _httpClientFactory.CreateClient();

        var token = User.FindFirst("AccessToken")?.Value;

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PutAsJsonAsync(
            "https://localhost:7108/api/Cart/update",
            new
            {
                CartItemId = cartItemId,
                Quantity = quantity
            });

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        var client = _httpClientFactory.CreateClient();

        var token = User.FindFirst("AccessToken")?.Value;

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync(
            $"https://localhost:7108/api/Cart/remove/{cartItemId}"
        );

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

}