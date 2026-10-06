using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace FeyzaStore.MVC.Controllers;

public class CheckoutController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CheckoutController(IHttpClientFactory httpClientFactory)
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

        var cart = await client.GetFromJsonAsync<CartViewModel>(
            "https://localhost:7108/api/Cart"
        );

        if (cart == null || !cart.Items.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        var addressResponse = await client.GetAsync(
    "https://localhost:7108/api/Address"
);

        if (addressResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return RedirectToAction("Create", "Address");
        }

        addressResponse.EnsureSuccessStatusCode();

        var address =
            await addressResponse.Content.ReadFromJsonAsync<AddressViewModel>();

        return View(address);

       
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder(int addressId)
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
            "https://localhost:7108/api/Order",
            new
            {
                AddressId = addressId
            });

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index", "Order");
    }
}