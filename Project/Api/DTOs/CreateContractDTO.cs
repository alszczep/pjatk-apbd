namespace Api.DTOs;

public class CreateContractDTO
{
    public Guid ClientId { get; set; }
    public Guid SoftwareProductId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int YearsOfExtendedSupport { get; set; }
}
