namespace api.Models;

public class ContractPayment
{
    public Guid Id { get; init; }
    public decimal PaymentAmountInPln { get; set; }

    public Contract Contract { get; set; } = null!;
}
