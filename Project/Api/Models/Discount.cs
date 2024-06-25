namespace api.Models;

public class Discount
{
    public static readonly decimal ReturningClientDiscountMultiplier = 0.95m;

    public Guid Id { get; init; }
    public int DiscountPercentage { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public SoftwareProduct SoftwareProduct { get; set; } = null!;
}
