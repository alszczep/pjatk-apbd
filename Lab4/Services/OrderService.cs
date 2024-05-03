using System.Data.SqlClient;
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

    public async Task<IEnumerable<Order>> GetNotFulfilledOrders(int productId, int amount, DateTime createdBefore)
    {
        return await this.orderRepository.GetNotFulfilledOrders(productId, amount, createdBefore);
    }

    public async Task FulfillOrder(int orderId, SqlCommand transactionCommand)
    {
        await this.orderRepository.FulfillOrder(orderId, transactionCommand);
    }
}
