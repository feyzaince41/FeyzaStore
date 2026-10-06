using System;
using System.Collections.Generic;
using System.Text;

namespace FeyzaStore.Application.DTOs;

public class AddressCreateDto
{
    public string Title { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string FullAddress { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;
}