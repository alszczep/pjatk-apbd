using Lab4.Model;

namespace Lab4.Services;

public interface IWarehouseService
{
    Task<Warehouse?> GetWarehouseById(int id);
}
