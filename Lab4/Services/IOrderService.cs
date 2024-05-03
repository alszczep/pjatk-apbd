using System.Data.SqlClient;
using Lab4.Model;

namespace Lab4.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetNotFulfilledOrders(int productId, int amount, DateTime createdBefore);
    Task FulfillOrder(int orderId, SqlCommand transactionCommand);
}
