using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lesson_11_Web_Api_Warehouses.Models;
using Lesson_11_Web_Api_Warehouses.Services;

namespace Lesson_11_Web_Api_Warehouses.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly ItemService itemService;
    private readonly WarehouseService warehouseService;

    public WarehouseController(ItemService itemService, WarehouseService warehouseService)
    {
        this.itemService = itemService;
        this.warehouseService = warehouseService;
    }

    [HttpPost]
    public IActionResult AddItemToWarehouseByIdAndQuantity(int itemId, int warehouseId, int quantity)
    {
        var result = warehouseService.AddItemToWarehouseByIdAndQuantity(itemId, warehouseId, quantity);

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpPost]
    public IActionResult RemoveItemFromWarehouseByIdAndQuantity(int itemId, int warehouseId, int quantity)
    {
        var result = warehouseService.RemoveItemFromWarehouseByIdAndQuantity(itemId, warehouseId, quantity);

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }


    [HttpPost]
    public IActionResult GetItemsByWarehouseId(int warehouseId)
    {
        // Retrieve the ItemCounts for the specified warehouse using the service
        var ItemsStored = warehouseService.GetItemsFromWarehouse(warehouseId);

        if (ItemsStored == null)
        {
            return NotFound("Warehouse not found.");
        }

        return Ok(ItemsStored);
    }

    [HttpPost]
    public WarehouseList GetWarehouses()
    {
        var result = warehouseService.GetWarehouses();

        return result;
    }


    [HttpPost]
    public IActionResult AddWarehouse([FromBody] AddWarehouse warehouse)
    {
        // Create a new Item instance using the itemService and add it to the items collection
        var result = warehouseService.AddWarehouse(warehouse);

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }
}
