using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class SalaryRangeCalculated
    {
        public string ExperienceYears { get; set; }
        public int MedianSalary { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
    }

}
