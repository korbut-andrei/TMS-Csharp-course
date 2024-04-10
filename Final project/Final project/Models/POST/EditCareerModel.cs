using Final_project.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models.POST
{
    public class EditCareerModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        [Required]
        public IFormFile? CareerImage { get; set; }
    }
}
