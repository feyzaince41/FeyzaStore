using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;
using FeyzaStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FeyzaStore.Infrastructure.Repositories;

public class ProductRepository
    : GenericRepository<Product>, IProductRepository
{
    private readonly FeyzaStoreDbContext _context;

    public ProductRepository(FeyzaStoreDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .ToListAsync();
    }
}