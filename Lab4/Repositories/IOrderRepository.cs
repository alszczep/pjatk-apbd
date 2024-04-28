using Lab4.Model;

namespace Lab4.Repositories;

public interface IOrderRepository
{
    IEnumerable<Order> GetNotFulfilledOrders(int productId, int amount, DateTime createdBefore);
    void FulfillOrder(int orderId);
}
