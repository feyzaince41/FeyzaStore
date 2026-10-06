using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
}