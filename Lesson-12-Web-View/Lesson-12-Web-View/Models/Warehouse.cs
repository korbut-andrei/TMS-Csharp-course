namespace Lesson_12_Web_View.Models
{
    public class Warehouse
    {
        private static int IdCounter = 1;

        public Warehouse()
        {
            WarehouseId = IdCounter;
            IdCounter++;
        }
        public int WarehouseId { get; set; } // Unique identifier for the warehouse
        public string Name { get; set; } // Name of the warehouse
                // Dictionary to store item counts (ItemId -> Count)
        public Dictionary<int, int> ItemsStored { get; set; } = new Dictionary<int, int>();
    }
}
