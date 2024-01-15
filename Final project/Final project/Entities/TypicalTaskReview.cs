using Final_project.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Entities
{
    public class TypicalTaskReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Description { get; set; }

        [ForeignKey("TypicalTaskId")]
        public int TypicalTaskId { get; set; }
        public virtual TypicalTaskEntity TypicalTaskEntity { get; set; }

        [ForeignKey("ReviewId")]
        public int ReviewId { get; set; }
        public virtual ReviewEntity ReviewEntity { get; set; }

        public virtual ICollection<TypicalTaskReview> ReviewBulletPoints { get; set; }

        public DateTime DateTimeInUtc { get; set; }

    }
}
