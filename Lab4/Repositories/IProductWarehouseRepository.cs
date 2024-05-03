using Lab4.Controllers.DTOs;
using Lab4.Helpers;
using Lab4.Model;

namespace Lab4.Repositories;

public interface IProductWarehouseRepository
{
    Task<IEnumerable<ProductWarehouse>> GetProductWarehouseListByOrderId(int orderId);
    Task<int> AddProductWarehouse(ProductWarehouse productWarehouse);
    Task<ResponseOrError<int>> AddProductWarehouseWithProcedure(AddProductToWarehouseDTO dto);
}
