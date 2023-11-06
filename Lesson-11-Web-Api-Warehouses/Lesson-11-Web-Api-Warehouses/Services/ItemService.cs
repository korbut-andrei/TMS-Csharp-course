using Lesson_11_Web_Api_Warehouses.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;

namespace Lesson_11_Web_Api_Warehouses.Services
{
    public class ItemService : IItemService
    {
        public List<Item> Items { get; } = new List<Item>();

        public CommandResultModel AddItem(AddItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Color))
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = "Color field is empty",
                };
            }

            /*if ((!item.HasRecliner is not null))
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = "HasRecliner field is empty or invalid value for boolean field",
                };
            }*/

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                return new CommandResultModel
                {
                    Success = false,
                    Message = "Name field is empty",
                };
            }

            Items.Add(new Item
            {
                Name = item.Name,
                Color = item.Color,
                HasRecliner = item.HasRecliner,
            });

            return new CommandResultModel
            {
                Success = true,
                Message = "New item has been sucessfully added",
            };
        }

        public Item GetItemById(int itemId)
        {
            return Items.FirstOrDefault(i => i.ItemId == itemId);
        }

        public ItemList GetItems()
        {
            return new ItemList
            {
                Items = Items.ToArray(),
                Count = Items.Count(),
            };
        }
    }
}
