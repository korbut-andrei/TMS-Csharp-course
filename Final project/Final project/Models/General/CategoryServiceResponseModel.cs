using Final_project.Entities;

namespace Final_project.Models.General
{
    public class CategoryServiceResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public CategoryEntity? Category { get; set; }
    }
}
