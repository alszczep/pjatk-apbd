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

    public async Task<IEnumerable<ProductWarehouse>> GetProductWarehouseListByOrderId(int orderId)
    {
        return await this.productWarehouseRepository.GetProductWarehouseListByOrderId(orderId);
    }

    public async Task<int> AddProductWarehouse(ProductWarehouse productWarehouse)
    {
        return await this.productWarehouseRepository.AddProductWarehouse(productWarehouse);
    }

    public async Task<ResponseOrError<int>> AddProductWarehouseWithProcedure(AddProductToWarehouseDTO dto)
    {
        return await this.productWarehouseRepository.AddProductWarehouseWithProcedure(dto);
    }
}
