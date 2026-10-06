namespace FeyzaStore.MVC.Models;

public class OrderViewModel
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = string.Empty;

    public int AddressId { get; set; }

    public List<OrderItemViewModel> Items { get; set; } = new();
}