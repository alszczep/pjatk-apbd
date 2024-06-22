namespace api.Models;

public class ClientIndividual : Client
{
    public string Pesel { get; init; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
}
