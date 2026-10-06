using Microsoft.EntityFrameworkCore;
using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;
using FeyzaStore.Infrastructure.Data;

namespace FeyzaStore.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly FeyzaStoreDbContext _context;

    public OrderRepository(FeyzaStoreDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
    {
        return await _context.Orders
            .Include(o => o.Items)
.ThenInclude(i => i.Product)
.ThenInclude(p => p.Images)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }
}