using Lab4.Controllers.DTOs;
using Lab4.Helpers;
using Lab4.Model;
using Lab4.Repositories;

namespace Lab4.Services;

public class ProductWarehouseService : IProductWarehouseService
{
    private readonly IProductWarehouseRepository productWarehouseRepository;

    public ProductWarehouseService(IProductWarehouseRepository productWarehouseRepository)
    {
        this.productWarehouseRepository = productWarehouseRepository;
    }

    public IEnumerable<ProductWarehouse> GetProductWarehouseListByOrderId(int orderId)
    {
        return this.productWarehouseRepository.GetProductWarehouseListByOrderId(orderId);
    }

    public int AddProductWarehouse(ProductWarehouse productWarehouse)
    {
        return this.productWarehouseRepository.AddProductWarehouse(productWarehouse);
    }

    public ResponseOrError<int> AddProductWarehouseWithProcedure(AddProductToWarehouseDTO dto)
    {
        return this.productWarehouseRepository.AddProductWarehouseWithProcedure(dto);
    }
}
