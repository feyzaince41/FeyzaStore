using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;
using FeyzaStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FeyzaStore.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly FeyzaStoreDbContext _context;

    public AddressRepository(FeyzaStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByUserIdAsync(string userId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}