namespace Api.DTOs;

public class CreateSubscriptionDTO
{
    public Guid ClientId { get; set; }
    public Guid SoftwareProductId { get; set; }
    public int RenewalPeriodInMonths { get; set; }
}
