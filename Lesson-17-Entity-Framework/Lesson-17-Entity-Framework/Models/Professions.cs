using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class Professions
    {
       [Key]
       public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int SalaryRangeMim { get; set; }

        public int SalaryRangeMax { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Reviews>? Reviews { get; set; }
    }
}
