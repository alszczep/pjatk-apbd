using Lab4.Controllers.DTOs;
using Lab4.Helpers;
using Lab4.Model;

namespace Lab4.Repositories;

public interface IProductWarehouseRepository
{
    IEnumerable<ProductWarehouse> GetProductWarehouseListByOrderId(int orderId);
    int AddProductWarehouse(ProductWarehouse productWarehouse);
    ResponseOrError<int> AddProductWarehouseWithProcedure(AddProductToWarehouseDTO dto);
}
