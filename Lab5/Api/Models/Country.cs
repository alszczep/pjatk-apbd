namespace Api.Models;

public partial class Country
{
    public int IdCountry { get; set; }
    public string Name { get; set; } = null!;
    public virtual Trip Trip { get; set; } = null!;
}
