using Lab4.Model;
using System.Data.SqlClient;

namespace Lab4.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration configuration;
    
    public ProductRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    public Product? GetProductById(int id)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdProduct, Name, Description, Price FROM [s24454].[dbo].[Product] WHERE IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", id);

        var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            return new Product
            {
                IdProduct = (int)dr["IdProduct"],
                Name = dr["Name"].ToString() ?? "",
                Description = dr["Description"].ToString() ?? "",
                Price = (float)dr["Price"]
            };
        }
        
        return null;
    }
}