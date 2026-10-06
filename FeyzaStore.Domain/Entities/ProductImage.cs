using System;
using System.Collections.Generic;
using System.Text;

namespace FeyzaStore.Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;
}