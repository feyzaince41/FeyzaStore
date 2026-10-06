using System;
using System.Collections.Generic;
using System.Text;

namespace FeyzaStore.Application.DTOs;

public class CartDto
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public List<CartItemDto> Items { get; set; } = new();
}
