using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AndreiKorbut.CareerChoiceBackend.Entities
{
    public class TypicalTaskEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("CareerId")]
        public int CareerId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }

        public virtual ICollection<TypicalTaskReview> ReviewBulletPoints { get; set; }
    }
}
