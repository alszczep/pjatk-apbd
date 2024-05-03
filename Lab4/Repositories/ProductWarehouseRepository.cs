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

    public async Task<IEnumerable<ProductWarehouse>> GetProductWarehouseListByOrderId(int orderId)
    {
        await using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();

        cmd.Connection = con;
        cmd.CommandText = "SELECT IdProductWarehouse, IdProduct, IdWarehouse, Amount, Price, CreatedAt FROM [s24454].[dbo].[Product_Warehouse] WHERE IdProductWarehouse = @IdProductWarehouse";
        cmd.Parameters.AddWithValue("@IdProductWarehouse", orderId);

        var dr = await cmd.ExecuteReaderAsync();
        var productWarehouses = new List<ProductWarehouse>();
        while (await dr.ReadAsync())
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

    public async Task<int> AddProductWarehouse(ProductWarehouse productWarehouse, SqlCommand transactionCommand)
    {
        transactionCommand.CommandText = "INSERT INTO [s24454].[dbo].[Product_Warehouse] (IdProduct, IdWarehouse, IdOrder, Amount, Price, CreatedAt) VALUES (@IdProduct, @IdWarehouse, @IdOrder, @Amount, @Price, @CreatedAt);SELECT SCOPE_IDENTITY();";
        transactionCommand.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        transactionCommand.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        transactionCommand.Parameters.AddWithValue("@IdOrder", productWarehouse.IdOrder);
        transactionCommand.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
        transactionCommand.Parameters.AddWithValue("@Price", productWarehouse.Price);
        transactionCommand.Parameters.AddWithValue("@CreatedAt", productWarehouse.CreatedAt);

        var newId = (decimal?) await transactionCommand.ExecuteScalarAsync();

        if(newId is null)
        {
            throw new Exception("Failed to get new id");
        }

        return (int)newId;
    }

    public async Task<ResponseOrError<int>> AddProductWarehouseWithProcedure(AddProductToWarehouseDTO dto)
    {
        await using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();

        cmd.Connection = con;
        cmd.CommandText = "AddProductToWarehouse";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
        cmd.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
        cmd.Parameters.AddWithValue("@Amount", dto.Amount);
        cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);

        try
        {
            var newId = (decimal?) await cmd.ExecuteScalarAsync();

            if(newId is null)
            {
                return new ResponseOrError<int>()
                {
                    Response = -1,
                    Error = "Failed to get new id"
                };
            }

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
