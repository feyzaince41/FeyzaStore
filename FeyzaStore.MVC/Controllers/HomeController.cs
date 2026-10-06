using FeyzaStore.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Json;

namespace FeyzaStore.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Ana Sayfa
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var products =
                await client.GetFromJsonAsync<List<ProductViewModel>>(
                    "https://localhost:7108/api/Product/with-category"
                );

            return View(products ?? new List<ProductViewModel>());
        }

        // Hakkımızda
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        // Gizlilik
        public IActionResult Privacy()
        {
            return View();
        }

        // Hata Sayfası
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId =
                    Activity.Current?.Id ??
                    HttpContext.TraceIdentifier
            });
        }
    }
}