namespace api.Models;

public class Subscription
{
    public Guid Id { get; init; }
    public DateOnly AddedDate { get; set; }
    public int RenewalPeriodInMonths { get; set; }
    public decimal BasePriceForRenewalPeriod { get; set; }

    public Client Client { get; set; } = null!;
    public SoftwareProduct SoftwareProduct { get; set; } = null!;
    public ICollection<SubscriptionPayment> Payments { get; set; } = new List<SubscriptionPayment>();
}
