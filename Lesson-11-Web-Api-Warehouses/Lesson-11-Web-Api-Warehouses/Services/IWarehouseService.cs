using Lesson_11_Web_Api_Warehouses.Models;

namespace Lesson_11_Web_Api_Warehouses.Services
{
    public interface IWarehouseService
    {
        WarehouseList GetWarehouses();
        CommandResultModel AddWarehouse(AddWarehouse warehouse);

        CommandResultModel AddItemToWarehouseById(int itemId, int warehouseId, int quantity);

        public Dictionary<int, int> GetItemsFromWarehouse(int warehouseId);
    }
}
