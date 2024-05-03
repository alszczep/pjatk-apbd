
using System.Data.SqlClient;
using Lab4.Model;

namespace Lab4.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetNotFulfilledOrders(int productId, int amount, DateTime createdBefore);
    Task FulfillOrder(int orderId, SqlCommand transactionCommand);
}
