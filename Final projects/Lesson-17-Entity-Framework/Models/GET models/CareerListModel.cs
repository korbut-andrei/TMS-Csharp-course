using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class CareerListModel
    {
        public string CareerName { get; set; }
        public SalaryRange SalaryRange { get; set; }
        public string Category { get; set; }
        public string ImgURL { get; set; }
        public AverageReviewAndReviewCount AverageReviewAndReviewCount { get; set; }
    }
}
