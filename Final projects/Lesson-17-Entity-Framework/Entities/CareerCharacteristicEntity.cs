using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson_17_Entity_Framework.Entities
{
    public class CareerCharacteristicEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        public string Type { get; set; }

        public int? AverageRating { get; set; }

        public string? AverageRatingString { get; set; }

        public int CareerEntityId { get; set; }
        public virtual UserEntity CareerEntity { get; set; }
    }
}
