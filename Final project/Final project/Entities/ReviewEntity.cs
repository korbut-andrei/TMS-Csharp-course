using Final_project.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Final_project.Entities
{
    public class ReviewEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }

        public decimal Rating { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual UserEntity UserEntity { get; set; }

        public bool IsApproved { get; set; }

        [ForeignKey("CareerId")]
        public int CareerId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }

        public virtual ICollection<ReviewBulletPointEntity> ReviewBulletPoints { get; } = new List<ReviewBulletPointEntity>();
        public DateTime DateTimeInUtc { get; set; }

    }
}
