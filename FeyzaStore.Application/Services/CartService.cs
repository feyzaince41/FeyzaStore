using FeyzaStore.Application.DTOs;
using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
      
    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;


    }

    public async Task<CartDto?> GetCartAsync(string userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
            return null;

        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,

            Items = cart.Items.Select(item => new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Product.Price,
                Quantity = item.Quantity,

                ImageUrl = item.Product.Images
            .OrderByDescending(i => i.Id)
            .Select(i => i.ImageUrl.StartsWith("/")
                ? i.ImageUrl
                : "/images/products/" + i.ImageUrl)
            .FirstOrDefault() ?? string.Empty

            }).ToList()
        };
    }
    public async Task AddToCartAsync(string userId, AddToCartDto dto)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);

        // Kullanıcının sepeti yoksa oluştur
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };

            await _cartRepository.AddCartAsync(cart);
        }

        // Ürün sepette zaten var mı?
        var existingItem = cart.Items
            .FirstOrDefault(i => i.ProductId == dto.ProductId);

        if (existingItem != null)
        {
            // Varsa miktarını artır
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            // Yoksa yeni CartItem oluştur
            cart.Items.Add(new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            });
        }

        await _cartRepository.SaveChangesAsync();
    }
    public async Task UpdateQuantityAsync(string userId, UpdateCartItemDto dto)
    {
        var cartItem = await _cartRepository.GetCartItemByIdAsync(dto.CartItemId);

        if (cartItem == null)
            return;

        if (cartItem.Cart.UserId != userId)
            return;

        if (dto.Quantity < 1)
            return;

        cartItem.Quantity = dto.Quantity;

        await _cartRepository.SaveChangesAsync();
    }
    public async Task RemoveFromCartAsync(string userId, int cartItemId)
    {
        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

        if (cartItem == null)
            return;

        if (cartItem.Cart.UserId != userId)
            return;

        _cartRepository.DeleteCartItem(cartItem);

        await _cartRepository.SaveChangesAsync();
    }
}