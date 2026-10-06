using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using FeyzaStore.MVC.Models;


namespace FeyzaStore.MVC.Controllers;

public class ProductController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();

        var products = await client.GetFromJsonAsync<List<ProductViewModel>>(
      "https://localhost:7108/api/Product/with-category"
  );

        return View(products ?? new List<ProductViewModel>());
    }
    public async Task<IActionResult> Detail(int id)
    {
        var client = _httpClientFactory.CreateClient();

        var products = await client.GetFromJsonAsync<List<ProductViewModel>>(
            "https://localhost:7108/api/Product/with-category"
        );

        var product = products?.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
}