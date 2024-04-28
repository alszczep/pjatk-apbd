using System.Data;
using Lab4.Model;
using System.Data.SqlClient;
using Lab4.Controllers.DTOs;
using Lab4.Helpers;

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
        cmd.CommandText = "SELECT IdProductWarehouse, IdProduct, IdWarehouse, Amount, Price, CreatedAt FROM [s24454].[dbo].[Product_Warehouse] WHERE IdProductWarehouse = @IdProductWarehouse";
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
                Price = (decimal)dr["Price"],
                CreatedAt = (DateTime)dr["CreatedAt"]
            });
        }

        return productWarehouses;
    }

    public int AddProductWarehouse(ProductWarehouse productWarehouse)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var addCmd = new SqlCommand();

        addCmd.Connection = con;
        addCmd.CommandText = "INSERT INTO [s24454].[dbo].[Product_Warehouse] (IdProduct, IdWarehouse, IdOrder, Amount, Price, CreatedAt) VALUES (@IdProduct, @IdWarehouse, @IdOrder, @Amount, @Price, @CreatedAt)";
        addCmd.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        addCmd.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        addCmd.Parameters.AddWithValue("@IdOrder", productWarehouse.IdOrder);
        addCmd.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
        addCmd.Parameters.AddWithValue("@Price", productWarehouse.Price);
        addCmd.Parameters.AddWithValue("@CreatedAt", productWarehouse.CreatedAt);

        addCmd.ExecuteNonQuery();

        using var newIdCmd = new SqlCommand();

        newIdCmd.Connection = con;
        newIdCmd.CommandText = "SELECT IdProductWarehouse FROM [s24454].[dbo].[Product_Warehouse] WHERE IdProduct = @IdProduct AND IdWarehouse = @IdWarehouse AND Amount = @Amount AND Price = @Price AND CreatedAt = @CreatedAt";
        newIdCmd.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        newIdCmd.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        newIdCmd.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
        newIdCmd.Parameters.AddWithValue("@Price", productWarehouse.Price);
        newIdCmd.Parameters.AddWithValue("@CreatedAt", productWarehouse.CreatedAt);

        var newId = (int)newIdCmd.ExecuteScalar();

        return newId;
    }

    public ResponseOrError<int> AddProductWarehouseWithProcedure(AddProductToWarehouseDTO dto)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();

        cmd.Connection = con;
        cmd.CommandText = "AddProductToWarehouse";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
        cmd.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
        cmd.Parameters.AddWithValue("@Amount", dto.Amount);
        cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);

        try
        {
            var newId = (decimal)cmd.ExecuteScalar();
            return new ResponseOrError<int>()
            {
                Response = (int)newId,
                Error = null
            };
        }
        catch (SqlException e)
        {
            return new ResponseOrError<int>()
            {
                Response = -1,
                Error = e.Message
            };
        }
    }
}
