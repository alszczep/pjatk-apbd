namespace api.Models;

public class ClientCompany : Client
{
    public string Krs { get; init; } = null!;
    public string CompanyName { get; set; } = null!;
}
