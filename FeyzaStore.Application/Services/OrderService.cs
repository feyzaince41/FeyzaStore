using FeyzaStore.Application.Interfaces;
using FeyzaStore.Application.DTOs;
using FeyzaStore.Domain.Entities;
namespace FeyzaStore.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;

    private readonly IAddressRepository _addressRepository;

    public OrderService(
    IOrderRepository orderRepository,
    ICartRepository cartRepository,
    IAddressRepository addressRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _addressRepository = addressRepository;
    }

    public async Task<bool> CreateOrderAsync(string userId, CreateOrderDto dto)
    {
        var address = await _addressRepository.GetByUserIdAsync(userId);
        if (address == null || address.Id != dto.AddressId)
            return false;
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null || !cart.Items.Any())
            return false;

        var order = new Order
        {
            UserId = userId,
            AddressId = dto.AddressId,
            OrderDate = DateTime.Now,
            Status = "Hazırlanıyor",

            TotalPrice = cart.Items.Sum(item =>
                item.Product.Price * item.Quantity),

            Items = cart.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Product.Price
            }).ToList()
        };

        await _orderRepository.AddAsync(order);

        await _orderRepository.SaveChangesAsync();

        _cartRepository.ClearCart(cart);
        await _cartRepository.SaveChangesAsync();
        return true;
    }

    public async Task<List<OrderDto>> GetOrdersAsync(string userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);

        return orders.Select(order => new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            AddressId = order.AddressId,
            Items = order.Items.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,

                ImageUrl = item.Product.Images
                    .OrderByDescending(i => i.Id)
                    .Select(i => i.ImageUrl.StartsWith("/")
                        ? i.ImageUrl
                        : "/images/products/" + i.ImageUrl)
                    .FirstOrDefault() ?? string.Empty

            }).ToList()

        }).ToList();
    }
}