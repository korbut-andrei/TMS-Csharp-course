using Lesson_12_Web_View.Models;
using System.Collections.Generic;

namespace Lesson_12_Web_View.Services
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

        public CommandResultModel AddItemToWarehouseByIdAndQuantity(int itemId, int warehouseId, int quantity)
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

        public CommandResultModel RemoveItemFromWarehouseByIdAndQuantity(int SelectedWarehouseId, int SelectedItemId, int quantity)
        {
            // Find the target warehouse by WarehouseId
            Warehouse targetWarehouse = Warehouses.SingleOrDefault(w => w.WarehouseId == SelectedWarehouseId);
            Item targetItem = itemService.GetItemById(SelectedItemId);

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
            if (targetWarehouse.ItemsStored.ContainsKey(SelectedItemId))
            {

                if (targetWarehouse.ItemsStored[SelectedItemId] < quantity)
                {
                    // Handle the case where the warehouse with the given WarehouseId doesn't exist
                    return new CommandResultModel
                    {
                        Success = false,
                        Message = "The input quantity is more than available quantity of given item.",
                    };
                }
                // Item exists; increase its count by Quantity
                targetWarehouse.ItemsStored[SelectedItemId] -= quantity;
                return new CommandResultModel
                {
                    Success = true,
                    Message = $"Count of {targetItem.Name}/{targetItem.Color} has been decreased by {quantity} in the {targetWarehouse.Name}. Remaining count: {targetWarehouse.ItemsStored[SelectedItemId]}",
                };
            }
            else
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = $"{targetItem.Name}/{targetItem.Color} is not present in the {targetWarehouse.Name}",
                };
            }
        }

        public CommandResultModel RemoveItemFromWarehouse(int itemId, int warehouseId)
        {
            // Find the target warehouse by WarehouseId
            Warehouse targetWarehouse = Warehouses.FirstOrDefault(w => w.WarehouseId == warehouseId);
            Item targetItem = itemService.GetItemById(itemId);

            if (targetItem == null)
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = "Item not found.",
                };
            }

            if (targetWarehouse == null)
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = "Warehouse not found.",
                };
            }

            if (targetWarehouse.ItemsStored.ContainsKey(itemId))
            {
                targetWarehouse.ItemsStored.Remove(itemId);
                return new CommandResultModel
                {
                    Success = true,
                    Message = $"{targetItem.Name}/{targetItem.Color} has been removed from {targetWarehouse.Name} ",
                };
            }
            else
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = $"{targetItem.Name}/{targetItem.Color} is not present in the {targetWarehouse.Name}",
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
