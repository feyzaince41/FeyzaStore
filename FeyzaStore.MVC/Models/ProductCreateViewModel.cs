using System.ComponentModel.DataAnnotations;

namespace FeyzaStore.MVC.Models;

public class ProductCreateViewModel
{
    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Açıklama zorunludur.")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stok 0'dan küçük olamaz.")]
    public int Stock { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Kategori seçmelisiniz.")]
    public int CategoryId { get; set; }

    public List<CategoryViewModel> Categories { get; set; } = new();
}