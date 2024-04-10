using Final_project.Entities;

namespace Final_project.Models.GET_models
{
    public class CategoryWithCareers
    {
        public CategoryEntity Category { get; set; }

        public CareerListModel[] Careers { get; set; }
    }
}
