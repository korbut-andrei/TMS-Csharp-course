using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class SearchResult
    {
        public List<Entities.UserEntity> MatchedCareers { get; set; }
        public List<CategoryEntity> MatchedCategories { get; set; }
    }
}
