using Api.Model;
using System.Data.SqlClient;

namespace Api.Repositories;

public class AnimalsRepository : IAnimalsRepository
{
    private IConfiguration configuration;
    
    public AnimalsRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    public IEnumerable<Animal> GetAnimals(string? orderBy)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        
        cmd.Connection = con;
        // orderBy has to be added without using a parameter, since parameters for ORDER BY are not allowed
        cmd.CommandText = "SELECT IdAnimal, Name, Description, Category, Area FROM [s24454].[dbo].[Animal] ORDER BY " + (orderBy ?? "Name");

        var dr = cmd.ExecuteReader();
        var animals = new List<Animal>();
        while (dr.Read())
        {
            var animal = new Animal
            {
                IdAnimal = (int)dr["IdAnimal"],
                Name = dr["Name"].ToString()!,
                Description = dr["Description"].ToString(),
                Category = dr["Category"].ToString()!,
                Area = dr["Area"].ToString()!,
            };
            animals.Add(animal);
        }
        
        return animals;
    }

    public int CreateAnimal(Animal animal)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO [s24454].[dbo].[Animal](IdAnimal, Name, Description, Category, Area) VALUES(@IdAnimal, @Name, @Description, @Category, @Area)";
        cmd.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);
        cmd.Parameters.AddWithValue("@Name", animal.Name);
        cmd.Parameters.AddWithValue("@Description", animal.Description);
        cmd.Parameters.AddWithValue("@Category", animal.Category);
        cmd.Parameters.AddWithValue("@Area", animal.Area);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }

    
    public int DeleteAnimal(int animalId)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "DELETE FROM [s24454].[dbo].[Animal] WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", animalId);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }

    public int UpdateAnimal(Animal animal)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE [s24454].[dbo].[Animal] SET Name=@Name, Description=@Description, Category=@Category, Area=@Area WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);
        cmd.Parameters.AddWithValue("@Name", animal.Name);
        cmd.Parameters.AddWithValue("@Description", animal.Description);
        cmd.Parameters.AddWithValue("@Category", animal.Category);
        cmd.Parameters.AddWithValue("@Area", animal.Area);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
}