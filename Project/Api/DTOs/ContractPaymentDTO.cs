namespace Api.DTOs;

public class ContractPaymentDTO
{
    public Guid ContractId { get; set; }
    public decimal PaymentAmountInPln { get; set; }
}
