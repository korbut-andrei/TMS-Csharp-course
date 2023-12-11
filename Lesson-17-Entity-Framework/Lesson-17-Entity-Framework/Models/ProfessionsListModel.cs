using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class ProfessionsListModel
    {
       public ProfessionsListEntryModel[] Professions { get; set; }
    }

    public class ProfessionsListEntryModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int SalaryRangeMin { get; set; }

        public int SalaryRangeMax { get; set; }

        [Required]
        public string Description { get; set; }

        public ProfessionsListReviewEntryModel[] Reviews { get; set; }

    }
}
