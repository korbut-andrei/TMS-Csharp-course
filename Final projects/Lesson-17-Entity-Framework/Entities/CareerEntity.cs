using Lesson_17_Entity_Framework.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson_17_Entity_Framework.Entities
{

    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ProfessionName { get; set; }
        public string Description { get; set; }
        public CategoryEntity Category { get; set; }
        public string ImgURL { get; set; }

    }
}
