using FeyzaStore.Application.DTOs;
using FeyzaStore.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FeyzaStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressController(AddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddressCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        await _addressService.AddAddressAsync(userId, dto);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAddress()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized();
        }

        var address = await _addressService.GetAddressAsync(userId);

        if (address == null)
        {
            return NotFound();
        }

        return Ok(address);
    }
}