
using FeyzaStore.Application.DTOs;
using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    // 1. Bütün ürünleri getir
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    // 2. ID'ye göre ürün getir
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    // 3. Yeni ürün ekle
    public async Task AddAsync(Product product)
    {
        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
    }

    // 4. Ürün güncelle
    public async Task<bool> UpdateAsync(Product product)
    {
        var existingProduct =
            await _repository.GetByIdAsync(product.Id);

        if (existingProduct == null)
            return false;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.CategoryId = product.CategoryId;

        _repository.Update(existingProduct);

        await _repository.SaveChangesAsync();

        return true;
    }

    // 5. Ürün sil
    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return false;

        _repository.Delete(product);

        await _repository.SaveChangesAsync();

        return true;
    }

    // 6. Ürünleri kategori ve fotoğraf bilgileriyle getir
    public async Task<IEnumerable<ProductListDto>>
        GetProductsWithCategoryAsync()
    {
        var products =
            await _repository.GetProductsWithCategoryAsync();

        return products.Select(p => new ProductListDto
        {
            Id = p.Id,

            Name = p.Name,

            Description = p.Description,

            Price = p.Price,

            Stock = p.Stock,

            CategoryId = p.CategoryId,

            CategoryName = p.Category.Name,

            ImageUrl = p.Images
    .Where(i =>
        i.ImageUrl.EndsWith(".jpg") ||
        i.ImageUrl.EndsWith(".jpeg") ||
        i.ImageUrl.EndsWith(".png"))
    .OrderByDescending(i => i.Id)
    .Select(i => i.ImageUrl.StartsWith("/")
        ? i.ImageUrl
        : "/images/products/" + i.ImageUrl)
    .FirstOrDefault() ?? string.Empty
        }).ToList();
    }
}