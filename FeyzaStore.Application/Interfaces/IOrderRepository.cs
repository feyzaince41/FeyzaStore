using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
    Task AddAsync(Order order);

    Task SaveChangesAsync();
}