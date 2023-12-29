using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lesson_17_Entity_Framework.Models
{
    public class CareerCharacteristicList
    {
        public CareerCharacteristicViewModel CareerCharacteristics { get; set; }

        public int ReviewsCount { get; set; }
    }
}
