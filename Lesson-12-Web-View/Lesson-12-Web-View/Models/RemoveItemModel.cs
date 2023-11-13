// AddItemViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lesson_12_Web_View.Models
{
    public class RemoveItemModel
    {
        public Warehouse WarehouseModel { get; set; }
        public Item ItemModel { get; set; }

        public List<SelectListItem> WarehouseList { get; set; }
        public List<SelectListItem> ItemList { get; set; }

        public int SelectedWarehouseId { get; set; }
        public int SelectedItemId { get; set; }
        public int Quantity { get; set; }
    }
}


