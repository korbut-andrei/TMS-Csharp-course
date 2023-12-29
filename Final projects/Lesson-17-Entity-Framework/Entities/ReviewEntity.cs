using Lesson_17_Entity_Framework.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Lesson_17_Entity_Framework.Entities
{
    public class ReviewEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }

        public string Rating { get; set; }

        public int UserId { get; set; }
        public virtual UserEntity UserEntity { get; set; }

        public virtual ICollection<string> PositiveBulletPoints { get; set; }

        public virtual ICollection<string> NegativeBulletPoints { get; set; }

        // Foreign key to CareerEntity
        public int CareerEntityId { get; set; }
        public virtual UserEntity CareerEntity { get; set; }
    }
}
