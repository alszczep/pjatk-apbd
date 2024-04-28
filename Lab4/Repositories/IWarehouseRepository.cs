using Lab4.Model;

namespace Lab4.Repositories;

public interface IWarehouseRepository
{
    Warehouse? GetWarehouseById(int id);
}