using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Lesson_17_Entity_Framework.Models
{
    public class Reviews
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }

        public int Rating { get; set; }

        [Required]
        public string ProfessionId { get; set; }

        public Professions Profession { get; set; }
    }
}
