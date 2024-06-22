namespace api.Models;

public class Contract
{
    public static readonly decimal SupportYearPriceInPln = 1000;

    public Guid Id { get; init; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal PriceInPlnAfterDiscounts { get; set; }
    public int YearsOfExtendedSupport { get; set; }
    public bool IsSigned { get; set; }
    public DateTime? SignedDate { get; set; }

    public Client Client { get; set; } = null!;
    public SoftwareProduct SoftwareProduct { get; set; } = null!;
    public ICollection<ContractPayment> Payments { get; set; } = new List<ContractPayment>();
}
