using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace FeyzaStore.MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
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
            "https://localhost:7108/api/Product"
        );

        return View(products ?? new List<ProductViewModel>());
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var client = _httpClientFactory.CreateClient();

        var categories = await client.GetFromJsonAsync<List<CategoryViewModel>>(
            "https://localhost:7108/api/Category"
        );

        var model = new ProductCreateViewModel
        {
            Categories = categories ?? new List<CategoryViewModel>()
        };

        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        var client = _httpClientFactory.CreateClient();

        if (!ModelState.IsValid)
        {
            model.Categories =
                await client.GetFromJsonAsync<List<CategoryViewModel>>(
                    "https://localhost:7108/api/Category"
                ) ?? new List<CategoryViewModel>();

            return View(model);
        }

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
            "https://localhost:7108/api/Product",
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
            $"https://localhost:7108/api/Product/{id}"
        );

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient();

        var product = await client.GetFromJsonAsync<ProductViewModel>(
            $"https://localhost:7108/api/Product/{id}"
        );

        if (product == null)
        {
            return NotFound();
        }

        var categories = await client.GetFromJsonAsync<List<CategoryViewModel>>(
    "https://localhost:7108/api/Category"
);

        product.Categories = categories ?? new List<CategoryViewModel>();
        return View(product);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductViewModel model)
    {
        var client = _httpClientFactory.CreateClient();

        if (!ModelState.IsValid)
        {
            model.Categories =
                await client.GetFromJsonAsync<List<CategoryViewModel>>(
                    "https://localhost:7108/api/Category"
                ) ?? new List<CategoryViewModel>();

            return View(model);
        }

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
            $"https://localhost:7108/api/Product/{model.Id}",
            model
        );

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult UploadImage(int id)
    {
        ViewBag.ProductId = id;

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadImage(int productId, IFormFile file)
    {

        if (file == null || file.Length == 0)
        {
            ViewBag.ProductId = productId;
            ViewBag.Error = "Lütfen bir resim seçiniz.";
            return View();
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

        using var content = new MultipartFormDataContent();

        using var fileContent = new StreamContent(file.OpenReadStream());

        content.Add(fileContent, "file", file.FileName);
        content.Add(new StringContent(productId.ToString()), "productId");

        var response = await client.PostAsync(
            "https://localhost:7108/api/ProductImage/upload",
            content
        );

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }
}