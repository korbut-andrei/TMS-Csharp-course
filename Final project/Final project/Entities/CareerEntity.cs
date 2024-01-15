using Final_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Entities
{
    public class CareerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual CategoryEntity CategoryEntity { get; set; }
        public string Base64ImageData { get; set; }
        public bool IsDeleted { get; set; }

    }
}
