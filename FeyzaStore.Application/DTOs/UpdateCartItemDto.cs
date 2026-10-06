using System;
using System.Collections.Generic;
using System.Text;

namespace FeyzaStore.Application.DTOs;

public class UpdateCartItemDto
{
    public int CartItemId { get; set; }

    public int Quantity { get; set; }
}
