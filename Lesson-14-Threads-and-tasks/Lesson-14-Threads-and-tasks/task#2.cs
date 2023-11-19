using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

//2.Сделать класс склада(инвентаря) из предыдущего задания потокобезопасным. Используйте lock,
//Сдавать нужно только класс, приложение ASP.NET подключать не обязательно.

public class Warehouse
{
    private static int IdCounter = 1;

    public Warehouse()
    {
        WarehouseId = IdCounter;
        IdCounter++;
    }
    public int WarehouseId { get; set; } // Unique identifier for the warehouse
    public string Name { get; set; } // Name of the warehouse
                                     // Dictionary to store item counts (ItemId -> Count)
    public Dictionary<int, int> ItemsStored { get; set; } = new Dictionary<int, int>();
}



[Route("api/[controller]/[action]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly ItemService itemService;
    private readonly WarehouseService warehouseService;
    private readonly object locker = new object();


    public WarehouseController(ItemService itemService, WarehouseService warehouseService)
    {
        this.itemService = itemService;
        this.warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddItemToWarehouseByIdAndQuantity(int itemId, int warehouseId, int quantity)
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

    public async Task<CommandResultModel> AddItemToWarehouseByIdAndQuantityAsync(int itemId, int warehouseId, int quantity)
    {
        await Task.Run(() =>
        {
            lock (locker) // Lock the dictionary for thread safety
            {
                Warehouse targetWarehouse = Warehouses.FirstOrDefault(w => w.WarehouseId == warehouseId);
                Item targetItem = itemService.GetItemById(itemId);

                if (targetItem == null)
                {
                    // Handle the case where the warehouse with the given WarehouseId doesn't exist
                    return new CommandResultModel
                    {
                        Success = false,
                        Message = "Item not found.",
                    };
                }

                if (targetWarehouse == null)
                {
                    // Handle the case where the warehouse with the given WarehouseId doesn't exist
                    return new CommandResultModel
                    {
                        Success = false,
                        Message = "Warehouse not found.",
                    };
                }

                // Check if the item with the given ItemId exists in the warehouse's ItemCounts
                if (targetWarehouse.ItemsStored.ContainsKey(itemId))
                {
                    // Item exists; increase its count by Quantity
                    targetWarehouse.ItemsStored[itemId] += quantity;
                    return new CommandResultModel
                    {
                        Success = true,
                        Message = $"Count of {targetItem.Name}/{targetItem.Color} has been increased by {quantity} in the {targetWarehouse.Name} ",
                    };
                }
                else
                {
                    // Item doesn't exist; add it with the specified count
                    targetWarehouse.ItemsStored[itemId] = quantity;
                    return new CommandResultModel
                    {
                        Success = true,
                        Message = $"{quantity} x {targetItem.Name}/{targetItem.Color} has been added to the {targetWarehouse.Name}",
                    };
                }
            }
        }
        return result;
    }
}

