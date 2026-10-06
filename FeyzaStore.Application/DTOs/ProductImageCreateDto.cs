using System;
using System.Collections.Generic;
using System.Text;

namespace FeyzaStore.Application.DTOs;

public class ProductImageCreateDto
{
    public int ProductId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
}