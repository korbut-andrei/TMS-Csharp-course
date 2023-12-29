using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class ProfessionAddDto
    {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Description { get; set; }
    }
}
