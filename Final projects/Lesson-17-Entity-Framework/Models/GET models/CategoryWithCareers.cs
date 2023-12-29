using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class CategoryWithCareers
    {
        public CategoryEntity Category { get; set; }

        public CareerListModel[] Careers { get; set; }
    }
}
