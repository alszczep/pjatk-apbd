using Lab4.Model;

namespace Lab4.Repositories;

public interface IProductWarehouseRepository
{
    IEnumerable<ProductWarehouse> GetProductWarehouseListByOrderId(int orderId);
    void AddProductWarehouse(ProductWarehouse productWarehouse);
}