using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Entities
{
    public class EducationOptionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string SchoolName { get; set; }

        public decimal Price { get; set; }

        public string Base64ImageData { get; set; }

        public string OptionUrl { get; set; }

        public int TimeLength { get; set; }

        [ForeignKey("CareerId")]
        public int CareerId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }
    }
}
