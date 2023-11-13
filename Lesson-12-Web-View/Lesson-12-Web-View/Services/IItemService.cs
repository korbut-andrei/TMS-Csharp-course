using Lesson_12_Web_View.Models;

namespace Lesson_12_Web_View.Services
{
    public interface IItemService
    {
        ItemList GetItems();
        CommandResultModel AddItem(AddItem item);

        Item GetItemById(int id);
    }
}
