using FeyzaStore.Application.DTOs;
using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Services;

public class AddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task AddAddressAsync(string userId, AddressCreateDto dto)
    {
        var address = new Address
        {
            UserId = userId,
            Title = dto.Title,
            City = dto.City,
            District = dto.District,
            FullAddress = dto.FullAddress,
            PostalCode = dto.PostalCode
        };

        await _addressRepository.AddAsync(address);

        await _addressRepository.SaveChangesAsync();
    }
    public async Task<Address?> GetAddressAsync(string userId)
    {
        return await _addressRepository.GetByUserIdAsync(userId);
    }
}