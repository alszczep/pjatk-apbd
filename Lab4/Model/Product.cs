using System.ComponentModel.DataAnnotations;

namespace Lab4.Model;

public class Product
{
    [Key]
    public int IdProduct { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = "";
    
    [Required]
    [MaxLength(200)]
    public float Price { get; set; }
}