using Lesson_12_Web_View.Models;

namespace Lesson_12_Web_View.Services
{
    public interface IWarehouseService
    {
        WarehouseList GetWarehouses();
        CommandResultModel AddWarehouse(AddWarehouse warehouse);

        CommandResultModel AddItemToWarehouseByIdAndQuantity(int itemId, int warehouseId, int quantity);

        public Dictionary<int, int> GetItemsFromWarehouse(int warehouseId);

        public CommandResultModel RemoveItemFromWarehouse(int itemId, int warehouseId);

        public CommandResultModel RemoveItemFromWarehouseByIdAndQuantity(int itemId, int warehouseId, int quantity);
    }
}
