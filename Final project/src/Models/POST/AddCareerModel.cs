using AndreiKorbut.CareerChoiceBackend.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AndreiKorbut.CareerChoiceBackend.Models.POST
{
    public class AddCareerModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public IFormFile CareerImage { get; set; }
    }
}
