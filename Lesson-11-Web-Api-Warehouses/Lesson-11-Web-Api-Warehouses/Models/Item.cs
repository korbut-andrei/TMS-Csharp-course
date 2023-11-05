namespace Lesson_11_Web_Api_Warehouses.Models
{
    public class Item
    {
        private static int IdCounter = 1;

        public Item() 
        {
            ItemId = IdCounter;
            IdCounter++;
        }

        public int ItemId { get; set; } 
        public string Name { get; set; } 
        public string Color { get; set; } 
        public bool HasRecliner { get; set; }
    }
}
