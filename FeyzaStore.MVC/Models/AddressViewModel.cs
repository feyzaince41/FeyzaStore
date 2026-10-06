namespace FeyzaStore.MVC.Models;

public class AddressViewModel
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string FullAddress { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;
}