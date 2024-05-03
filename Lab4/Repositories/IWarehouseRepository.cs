using Lab4.Model;

namespace Lab4.Repositories;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetWarehouseById(int id);
}
