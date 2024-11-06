using AndreiKorbut.CareerChoiceBackend.Entities;

namespace AndreiKorbut.CareerChoiceBackend.Models.General
{
    public class CategoryServiceResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public CategoryEntity? Category { get; set; }
    }
}
