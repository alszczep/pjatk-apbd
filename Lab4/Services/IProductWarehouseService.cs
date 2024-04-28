using Lab4.Model;

namespace Lab4.Services;

public interface IProductWarehouseService
{
    IEnumerable<ProductWarehouse> GetProductWarehouseListByOrderId(int orderId);
    void AddProductWarehouse(ProductWarehouse productWarehouse);
}