using Lab4.Model;

namespace Lab4.Services;

public interface IWarehouseService
{
    Warehouse? GetWarehouseById(int id);
}