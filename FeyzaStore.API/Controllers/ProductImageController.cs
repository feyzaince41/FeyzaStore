using FeyzaStore.Application.DTOs;
using FeyzaStore.Application.Services;
using FeyzaStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FeyzaStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductImageController : ControllerBase
{
    private readonly ProductImageService _service;
    private readonly IWebHostEnvironment _environment;

    public ProductImageController(
        ProductImageService service,
        IWebHostEnvironment environment)
    {
        _service = service;
        _environment = environment;
    }

    // 1. Görsel adresi ekleme
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Add(ProductImageCreateDto dto)
    {
        var image = new ProductImage
        {
            ProductId = dto.ProductId,
            ImageUrl = dto.ImageUrl
        };

        await _service.AddAsync(image);

        return Ok(new
        {
            image.Id,
            image.ProductId,
            image.ImageUrl
        });
    }

    // 2. Ürüne ait görselleri listeleme
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetByProductId(int productId)
    {
        var images = await _service.GetByProductIdAsync(productId);

        return Ok(images.Select(i => new
        {
            i.Id,
            i.ProductId,
            i.ImageUrl
        }));
    }

    // 3. Görsel kaydını silme
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // 4. Bilgisayardan gerçek fotoğraf yükleme
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] int productId)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Lütfen bir fotoğraf seçin.");
        }

        // Dosyanın uzantısını kontrol et
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(
                "Yalnızca JPG, JPEG ve PNG fotoğrafları yükleyebilirsiniz.");
        }

        // Dosya boyutunu kontrol et (en fazla 5 MB)
        if (file.Length > 5 * 1024 * 1024)
        {
            return BadRequest("Fotoğraf en fazla 5 MB olabilir.");
        }

        // Fotoğrafa benzersiz isim ver
        var fileName = Guid.NewGuid().ToString() + extension;

        // API projesinin wwwroot klasörünü bul
        var webRootPath = _environment.WebRootPath;

        if (string.IsNullOrEmpty(webRootPath))
        {
            webRootPath = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot"
            );
        }

        // Fotoğrafların kaydedileceği klasör
        var folderPath = Path.Combine(
            webRootPath,
            "images",
            "products"
        );

        Directory.CreateDirectory(folderPath);

        // Fotoğrafın kaydedileceği tam adres
        var filePath = Path.Combine(folderPath, fileName);

        // Konsolda klasör adresini göster
        Console.WriteLine(
            "Fotoğrafın kaydedildiği klasör: " + folderPath
        );

        // Fotoğrafı klasöre kaydet
        using (var stream = new FileStream(
            filePath,
            FileMode.CreateNew))
        {
            await file.CopyToAsync(stream);
        }

        // Veritabanına kaydedilecek görsel adresi
        var imageUrl = "/images/products/" + fileName;

        var image = new ProductImage
        {
            ProductId = productId,
            ImageUrl = imageUrl
        };

        await _service.AddAsync(image);

        return Ok(new
        {
            image.Id,
            image.ProductId,
            image.ImageUrl
        });
    }
}