using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson_17_Entity_Framework.Entities
{
    public class EducationOptionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string SchoolName { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string OptionUrl { get; set; }

        public int TimeLength { get; set; }

        public int CareerEntityId { get; set; }
        public virtual UserEntity CareerEntity { get; set; }
    }
}
