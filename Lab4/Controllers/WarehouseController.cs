using Lab4.Controllers.DTOs;
using Lab4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private IProductService productService;
    private IWarehouseService warehouseService;
    private IProductWarehouseService productWarehouseService;
    private IOrderService orderService;
    
    public WarehouseController(
        IProductService productService,
        IWarehouseService warehouseService,
        IProductWarehouseService productWarehouseService,
        IOrderService orderService)
    {
        this.productService = productService;
        this.warehouseService = warehouseService;
        this.productWarehouseService = productWarehouseService;
        this.orderService = orderService;
    }
    
    [HttpPost]
    public IActionResult AddProductToWarehouse(AddProductToWarehouseDTO dto)
    {
        var affectedCount = productWarehouseService.Create(dto);
        return StatusCode(StatusCodes.Status201Created);
    }
}

// 1. Sprawdzamy, czy produkt o podanym identyfikatorze istnieje. Następnie
// sprawdzamy, czy magazyn o podanym identyfikatorze istnieje. Wartość
// ilości przekazana w żądaniu powinna być większa niż 0.
// 2. Możemy dodać produkt do magazynu tylko wtedy, gdy istnieje
// zamówienie zakupu produktu w tabeli Order. Dlatego sprawdzamy, czy w
// tabeli Order istnieje rekord z IdProduktu i Ilością (Amount), które
// odpowiadają naszemu żądaniu. Data utworzenia zamówienia powinna
// być wcześniejsza niż data utworzenia w żądaniu.
// 3. Sprawdzamy, czy to zamówienie zostało przypadkiem zrealizowane.
// Sprawdzamy, czy nie ma wiersza z danym IdOrder w tabeli
// Product_Warehouse.
// 4. Aktualizujemy kolumnę FullfilledAt zamówienia na aktualną datę i
// godzinę. (UPDATE)
// 5. Wstawiamy rekord do tabeli Product_Warehouse. Kolumna Price
// powinna odpowiadać cenie produktu pomnożonej przez kolumnę Amount
// z naszego zamówienia. Ponadto wstawiamy wartość CreatedAt zgodnie
// z aktualnym czasem. (INSERT)
// 6. W wyniku operacji zwracamy wartość klucza głównego wygenerowanego
// dla rekordu wstawionego do tabeli Product_Warehouse.