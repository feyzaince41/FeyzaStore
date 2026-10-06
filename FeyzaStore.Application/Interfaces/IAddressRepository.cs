using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Interfaces;

public interface IAddressRepository
{
    Task<Address?> GetByUserIdAsync(string userId);

    Task AddAsync(Address address);

    Task SaveChangesAsync();
}
