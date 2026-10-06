using FeyzaStore.Domain.Entities;

namespace FeyzaStore.Application.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(string userId);

    Task<CartItem?> GetCartItemByIdAsync(int cartItemId);

    Task AddCartAsync(Cart cart);

    void DeleteCartItem(CartItem cartItem);

    void ClearCart(Cart cart);
    Task SaveChangesAsync();
}