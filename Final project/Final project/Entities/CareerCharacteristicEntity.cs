using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Entities
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

        [ForeignKey("CareerId")]
        public int CareerId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }
    }
}
