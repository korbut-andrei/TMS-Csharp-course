using AndreiKorbut.CareerChoiceBackend.Entities;

namespace AndreiKorbut.CareerChoiceBackend.Models.GET_models
{
    public class SearchResult
    {
        public List<CareerEntity> MatchedCareers { get; set; }
        public List<CategoryEntity> MatchedCategories { get; set; }
    }
}
