namespace api.Models;

public enum ClientType
{
    Individual = 0,
    Company = 1
}

public abstract class Client
{
    public Guid Id { get; init; }
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public ClientType Type { get; init; }

    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
