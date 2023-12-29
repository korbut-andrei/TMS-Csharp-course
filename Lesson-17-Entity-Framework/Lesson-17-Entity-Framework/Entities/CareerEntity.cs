using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson_17_Entity_Framework.Entities
{
    public class CareerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int SalaryRangeMim { get; set; }

        public int SalaryRangeMax { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<ReviewEntity>? Reviews { get; set; }
    }
}
