namespace FeyzaStore.MVC.Models;

public class LoginResponseViewModel
{
    public string UserId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();
}