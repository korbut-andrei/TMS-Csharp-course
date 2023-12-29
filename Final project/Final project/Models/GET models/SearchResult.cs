using Final_project.Entities;

namespace Final_project.Models.GET_models
{
    public class SearchResult
    {
        public List<CareerEntity> MatchedCareers { get; set; }
        public List<CategoryEntity> MatchedCategories { get; set; }
    }
}
