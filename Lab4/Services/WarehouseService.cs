using Lab4.Model;
using Lab4.Repositories;

namespace Lab4.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        this.warehouseRepository = warehouseRepository;
    }

    public async Task<Warehouse?> GetWarehouseById(int id)
    {
        return await this.warehouseRepository.GetWarehouseById(id);
    }
}
