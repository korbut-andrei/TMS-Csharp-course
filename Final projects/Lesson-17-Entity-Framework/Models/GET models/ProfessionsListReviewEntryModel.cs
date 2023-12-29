using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{

    public class ProfessionsListReviewEntryModel
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public int Rating { get; set; }

    }
}
