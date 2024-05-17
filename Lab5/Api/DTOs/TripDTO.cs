namespace Api.DTOs;

public class TripDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<TripCountryDTO> Countries { get; set; } = [];
    public List<TripClientDTO> Clients { get; set; } = [];
}

public class TripCountryDTO
{
    public string Name { get; set; } = null!;
}

public class TripClientDTO
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
