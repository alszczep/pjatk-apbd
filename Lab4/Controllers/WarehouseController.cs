using Lab4.Controllers.DTOs;
using Lab4.Helpers;
using Lab4.Model;
using Lab4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IProductService productService;
    private readonly IWarehouseService warehouseService;
    private readonly IProductWarehouseService productWarehouseService;
    private readonly IOrderService orderService;

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

    [HttpPost("code")]
    public IActionResult AddProductToWarehouseWithCode(AddProductToWarehouseDTO dto)
    {
        Product? product = this.productService.GetProductById(dto.IdProduct);
        if(product is null)
        {
            return this.StatusCode(StatusCodes.Status422UnprocessableEntity, "Product does not exist");
        }

        if(this.warehouseService.GetWarehouseById(dto.IdWarehouse) is null)
        {
            return this.StatusCode(StatusCodes.Status422UnprocessableEntity, "Warehouse does not exist");
        }

        if(dto.Amount <= 0)
        {
            return this.StatusCode(StatusCodes.Status422UnprocessableEntity, "Amount must be greater than 0");
        }

        List<Order> orders = this.orderService.GetNotFulfilledOrders(dto.IdProduct, dto.Amount, dto.CreatedAt).ToList();

        if(orders.Count == 0)
        {
            return this.StatusCode(StatusCodes.Status422UnprocessableEntity, "Order does not exist");
        }

        Order order = orders.First();

        List<ProductWarehouse> productWarehouseList = this.productWarehouseService.GetProductWarehouseListByOrderId(order.IdOrder).ToList();

        if(productWarehouseList.Count > 0)
        {
            return this.StatusCode(StatusCodes.Status422UnprocessableEntity, "Order has already been fulfilled");
        }

        this.orderService.FulfillOrder(order.IdOrder);
        int newId = this.productWarehouseService.AddProductWarehouse(
            new ProductWarehouse()
            {
                IdProduct = dto.IdProduct,
                IdWarehouse = dto.IdWarehouse,
                Amount = dto.Amount,
                Price = dto.Amount * product.Price,
                CreatedAt = DateTime.Now,
                IdOrder = order.IdOrder
            }
        );

        return this.StatusCode(StatusCodes.Status201Created, newId);
    }

    [HttpPost("procedure")]
    public IActionResult AddProductToWarehouseWithProcedure(AddProductToWarehouseDTO dto)
    {
        ResponseOrError<int> result = this.productWarehouseService.AddProductWarehouseWithProcedure(dto);

        if(result.Error is not null)
        {
            return this.StatusCode(StatusCodes.Status422UnprocessableEntity, result.Error);
        }

        return this.StatusCode(StatusCodes.Status201Created, result.Response);
    }
}
