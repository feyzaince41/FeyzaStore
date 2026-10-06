using System;
using System.Collections.Generic;
using System.Text;
using FeyzaStore.Application.DTOs;

namespace FeyzaStore.Application.DTOs;

public class UserListDto
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }
}