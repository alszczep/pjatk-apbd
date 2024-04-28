using Lab4.Model;
using Lab4.Repositories;

namespace Lab4.Services;

public class ProductWarehouseService : IProductWarehouseRepository
{
    private readonly IProductWarehouseRepository productWarehouseRepository;
    
    public ProductWarehouseService(IProductWarehouseRepository productWarehouseRepository)
    {
        this.productWarehouseRepository = productWarehouseRepository;
    }
    
    public IEnumerable<ProductWarehouse> GetProductWarehouseListByOrderId(int orderId)
    {
        return productWarehouseRepository.GetProductWarehouseListByOrderId(orderId);
    }
    
    public void AddProductWarehouse(ProductWarehouse productWarehouse)
    {
        productWarehouseRepository.AddProductWarehouse(productWarehouse);
    }
}
