using Lab4.Model;
using System.Data.SqlClient;

namespace Lab4.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<Warehouse?> GetWarehouseById(int id)
    {
        await using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();

        cmd.Connection = con;
        cmd.CommandText = "SELECT IdWarehouse, Name, Address FROM [s24454].[dbo].[Warehouse] WHERE IdWarehouse = @IdWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", id);

        var dr = await cmd.ExecuteReaderAsync();
        if (await dr.ReadAsync())
        {
            return new Warehouse
            {
                IdWarehouse = (int)dr["IdWarehouse"],
                Name = dr["Name"].ToString() ?? "",
                Address = dr["Address"].ToString() ?? ""
            };
        }

        return null;
    }
}
