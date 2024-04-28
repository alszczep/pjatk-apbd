using Lab4.Model;

namespace Lab4.Services;

public interface IOrderService
{
    IEnumerable<Order> GetNotFulfilledOrders(int productId, int amount, DateTime createdBefore);
    void FulfillOrder(int orderId);
}
