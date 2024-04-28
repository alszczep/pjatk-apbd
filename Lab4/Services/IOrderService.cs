using Lab4.Model;

namespace Lab4.Services;

public interface IOrderService
{
    IEnumerable<Order> GetOrders(int productId, int amount, DateTime createdBefore);
}