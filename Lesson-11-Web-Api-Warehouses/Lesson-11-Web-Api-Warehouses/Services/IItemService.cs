using Lesson_11_Web_Api_Warehouses.Models;

namespace Lesson_11_Web_Api_Warehouses.Services
{
    public interface IItemService
    {
        ItemList GetItems();
        CommandResultModel AddItem(AddItem item);

        Item GetItemById(int id);
    }
}
