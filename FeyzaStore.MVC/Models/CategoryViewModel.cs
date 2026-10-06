using System.ComponentModel.DataAnnotations;

namespace FeyzaStore.MVC.Models;


public class CategoryViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    public string Name { get; set; } = string.Empty;
}