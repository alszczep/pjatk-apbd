namespace api.Models;

public class SoftwareProduct
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public string Category { get; set; }
    public decimal UpfrontYearlyPriceInPln { get; set; }

    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
