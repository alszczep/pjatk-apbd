using Lab4.Model;
using Lab4.Repositories;

namespace Lab4.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository;
    }

    public IEnumerable<Order> GetNotFulfilledOrders(int productId, int amount, DateTime createdBefore)
    {
        return this.orderRepository.GetNotFulfilledOrders(productId, amount, createdBefore);
    }

    public void FulfillOrder(int orderId)
    {
        this.orderRepository.FulfillOrder(orderId);
    }
}
