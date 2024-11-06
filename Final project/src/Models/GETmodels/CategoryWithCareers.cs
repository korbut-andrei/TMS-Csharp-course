using AndreiKorbut.CareerChoiceBackend.Entities;

namespace AndreiKorbut.CareerChoiceBackend.Models.GET_models
{
    public class CategoryWithCareers
    {
        public CategoryEntity Category { get; set; }

        public CareerListModel[] Careers { get; set; }
    }
}
