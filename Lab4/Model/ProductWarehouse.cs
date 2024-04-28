using System.ComponentModel.DataAnnotations;

namespace Lab4.Model;

public class ProductWarehouse
{
    [Key]
    public int IdProductWarehouse { get; set; }

    [Required]
    public int IdWarehouse { get; set; }

    [Required]
    public int IdProduct { get; set; }

    [Required]
    public int IdOrder { get; set; }

    [Required]
    public int Amount { get; set; }

    [Required]
    [MaxLength(200)]
    public decimal Price { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
}
