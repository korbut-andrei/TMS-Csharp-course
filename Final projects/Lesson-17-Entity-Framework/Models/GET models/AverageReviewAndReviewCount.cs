using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class AverageReviewAndReviewCount
    {
        public float AverageReview { get; set; }
        public int ReviewCount { get; set; }
    }
}
