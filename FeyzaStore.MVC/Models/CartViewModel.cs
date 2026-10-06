namespace FeyzaStore.MVC.Models;

public class CartViewModel
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public List<CartItemViewModel> Items { get; set; } = new();
}
