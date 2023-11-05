using Lesson_11_Web_Api_Warehouses.Models;

namespace Lesson_11_Web_Api_Warehouses.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly ItemService itemService;
        public WarehouseService(ItemService itemService)
        {
            this.itemService = itemService;
        }
        public List<Warehouse> Warehouses { get; } = new List<Warehouse>();

        public CommandResultModel AddWarehouse(AddWarehouse warehouse)
        {
            if (string.IsNullOrWhiteSpace(warehouse.Name) )
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = "Name field is empty",
                };
            }

            if (Warehouses.Any(item => item.Name == warehouse.Name))
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = "There's already a warehouse with such name",
                };
            }
            else

            Warehouses.Add(new Warehouse
            {
                Name = warehouse.Name,
            });

            return new CommandResultModel
            {
                Success = true,
                Message = "New warehouse has been sucessfully added",
            };
        }

        public WarehouseList GetWarehouses()
        {
            return new WarehouseList
            {
                Warehouses = Warehouses.ToArray(),
                Count = Warehouses.Count(),
            };
        }

        public CommandResultModel AddItemToWarehouseById(int itemId, int warehouseId, int quantity)
        {
            // Find the target warehouse by WarehouseId
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

        public Dictionary<int, int> GetItemsFromWarehouse(int warehouseId)
        {
            Warehouse targetWarehouse = Warehouses.FirstOrDefault(w => w.WarehouseId == warehouseId);
            if (targetWarehouse != null)
            {
                return targetWarehouse.ItemsStored;
            }
            // Handle the case where the warehouse with the given warehouseId doesn't exist
            return null;
        }
    }
}
