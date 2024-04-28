using Lab4.Model;
using System.Data.SqlClient;

namespace Lab4.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IConfiguration configuration;
    
    public OrderRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    public IEnumerable<Order> GetOrders(int productId, int amount, DateTime createdBefore)
    {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdOrder, IdProduct, Amount, CreatedAt, FulfilledAt FROM [s24454].[dbo].[Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedBefore";
        cmd.Parameters.AddWithValue("@IdProduct", productId);
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@CreatedBefore", createdBefore);

        var dr = cmd.ExecuteReader();
        var orders = new List<Order>();
        while (dr.Read())
        {
            orders.Add(new Order
            {
                IdOrder = (int)dr["IdOrder"],
                IdProduct = (int)dr["IdProduct"],
                Amount = (int)dr["Amount"],
                CreatedAt = (DateTime)dr["CreatedAt"],
                FulfilledAt = (DateTime?)dr["FulfilledAt"]
            });
        }
        
        return orders;
    }
}