using AndreiKorbut.CareerChoiceBackend.Entities;

namespace AndreiKorbut.CareerChoiceBackend.Models.GET_models
{
    public class CareerWithRating
    {
        public int CareerId { get; set; } 
        public CareerEntity Career { get; set; }
        public decimal AverageRating { get; set; }
    }
}
