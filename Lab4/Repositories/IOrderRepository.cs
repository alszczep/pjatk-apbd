using Lab4.Model;

namespace Lab4.Repositories;

public interface IOrderRepository
{
    IEnumerable<Order> GetOrders(int productId, int amount, DateTime createdBefore);
}