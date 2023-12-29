using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_project.Models.POST
{
    public class AddCategoryModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IFormFile CategoryImage { get; set; }
    }
}
