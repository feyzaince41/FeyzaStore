using System;
using System.Collections.Generic;
using System.Text;

namespace FeyzaStore.Application.DTOs;

public class OrderDto
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = string.Empty;

    public int AddressId { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}