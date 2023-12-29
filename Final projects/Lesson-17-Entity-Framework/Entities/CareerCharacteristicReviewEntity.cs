using Lesson_17_Entity_Framework.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson_17_Entity_Framework.Entities
{
    public class CareerCharacteristicReviewEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Text { get; set; }

        public int? Rating { get; set; }

        public string? RatingString { get; set; }

        public int CareerCharacteristicId { get; set; }
        public virtual CareerCharacteristicEntity CareerCharacteristicEntity { get; set; }

        public int UserId { get; set; }
        public virtual UserEntity UserEntity { get; set; }
    }
}
