using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class CareerCharacteristicViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        public int AverageRating { get; set; }

        public string AverageRatingString { get; set; }

    }
}
