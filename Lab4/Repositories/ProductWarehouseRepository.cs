using Lab4.Model;
using System.Data.SqlClient;

namespace Lab4.Repositories;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    private readonly IConfiguration configuration;
    
    public ProductWarehouseRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    public IEnumerable<ProductWarehouse> GetProductWarehouseListByOrderId(int orderId)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdProductWarehouse, IdProduct, IdWarehouse, Amount, Price, CreatedAt FROM [s24454].[dbo].[ProductWarehouse] WHERE IdProductWarehouse = @IdProductWarehouse";
        cmd.Parameters.AddWithValue("@IdProductWarehouse", orderId);

        var dr = cmd.ExecuteReader();
        var productWarehouses = new List<ProductWarehouse>();
        while (dr.Read())
        {
            productWarehouses.Add(new ProductWarehouse
            {
                IdProductWarehouse = (int)dr["IdProductWarehouse"],
                IdProduct = (int)dr["IdProduct"],
                IdWarehouse = (int)dr["IdWarehouse"],
                Amount = (int)dr["Amount"],
                Price = (float)dr["Price"],
                CreatedAt = (DateTime)dr["CreatedAt"]
            });
        }
        
        return productWarehouses;   
    }

    public void AddProductWarehouse(ProductWarehouse productWarehouse)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO [s24454].[dbo].[ProductWarehouse] (IdProduct, IdWarehouse, Amount) VALUES (@IdProduct, @IdWarehouse, @Amount)";
        cmd.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        cmd.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        cmd.Parameters.AddWithValue("@Amount", productWarehouse.Amount);

        cmd.ExecuteNonQuery();
    }
}