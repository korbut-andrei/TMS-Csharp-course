using Final_project.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Final_project.Entities
{
    public class ReviewBulletPointEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }

        public string Type { get; set; }

        [ForeignKey("ReviewId")]
        public int ReviewId { get; set; }
        public virtual ReviewEntity ReviewEntity { get; set; }
    }
}
