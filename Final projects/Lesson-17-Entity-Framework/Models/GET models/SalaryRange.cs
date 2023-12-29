using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class SalaryRange
    {
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
    }
}
