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

        public string Rating { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public virtual UserEntity UserEntity { get; set; }

        [ForeignKey("CareerId")]
        public int CareerId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }
    }
}
