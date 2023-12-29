using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class ProfessionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

       public string Description { get; set; }
    }
}
