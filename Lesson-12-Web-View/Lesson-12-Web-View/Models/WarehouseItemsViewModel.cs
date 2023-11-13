using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lesson_12_Web_View.Models
{
    public class WarehouseItemsViewModel
    {
        public int SelectedWarehouseId { get; set; }
        public List<SelectListItem> WarehouseList { get; set; }
        public Dictionary<int, int> ItemsStored { get; set; }
    }
}