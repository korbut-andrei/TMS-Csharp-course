using Lesson_17_Entity_Framework.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson_17_Entity_Framework.Entities
{
    public class SalaryReportEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Salary { get; set; }
        public string ExperienceYears { get; set; }

        public int CareerEntityId { get; set; }
        public virtual UserEntity CareerEntity { get; set; }
    }
}
