namespace Api.DTOs;

public class RevenueDTO
{
    public Guid? ForClientId { get; set; }
    public Guid? ForSoftwareProductId { get; set; }
    public string? ForCurrency { get; set; }
}
