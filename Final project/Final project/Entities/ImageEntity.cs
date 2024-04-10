using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_project.Entities
{
    public class ImageEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Base64ImageData { get; set; }

        public virtual ICollection<CareerEntity> Careers { get; set; }
        public virtual ICollection<CategoryEntity> Categories { get; set; }
    }
}
