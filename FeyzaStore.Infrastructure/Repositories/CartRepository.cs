using FeyzaStore.Application.Interfaces;
using FeyzaStore.Domain.Entities;
using FeyzaStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FeyzaStore.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly FeyzaStoreDbContext _context;

    public CartRepository(FeyzaStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
    public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
    {
        return await _context.CartItems
        .Include(i => i.Cart)
        .FirstOrDefaultAsync(i => i.Id == cartItemId);
    }

    public async Task AddCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }
    public void ClearCart(Cart cart)
    {
        _context.CartItems.RemoveRange(cart.Items);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public void DeleteCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

}