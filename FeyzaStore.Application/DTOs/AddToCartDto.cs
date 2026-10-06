using System;
using System.Collections.Generic;
using System.Text;

namespace FeyzaStore.Application.DTOs;

public class AddToCartDto
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}