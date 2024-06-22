namespace Api.DTOs;

public class AddClientDTO
{
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public ClientTypeDTO Type { get; set; }

    public string? Pesel { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Krs { get; set; }
    public string? CompanyName { get; set; }
}
