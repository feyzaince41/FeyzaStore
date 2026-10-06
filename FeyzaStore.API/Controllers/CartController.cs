using FeyzaStore.Application.Services;
using Microsoft.AspNetCore.Mvc;
using FeyzaStore.Application.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FeyzaStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase

{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart(AddToCartDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        await _cartService.AddToCartAsync(userId, dto);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        var cart = await _cartService.GetCartAsync(userId);

        return Ok(cart);
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateQuantity(UpdateCartItemDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        await _cartService.UpdateQuantityAsync(userId, dto);

        return Ok();
    }
    [HttpDelete("remove/{cartItemId}")]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        await _cartService.RemoveFromCartAsync(userId, cartItemId);

        return Ok();
    }

}