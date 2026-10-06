using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Services;

public class ProductImageService
{
    private readonly IGenericRepository<ProductImage> _repository;

    public ProductImageService(
        IGenericRepository<ProductImage> repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(ProductImage image)
    {
        await _repository.AddAsync(image);
        await _repository.SaveChangesAsync();
    }
    public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId)
    {
        var images = await _repository.GetAllAsync();

        return images.Where(i => i.ProductId == productId).ToList();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var image = await _repository.GetByIdAsync(id);

        if (image == null)
            return false;

        _repository.Delete(image);

        await _repository.SaveChangesAsync();

        return true;
    }

}