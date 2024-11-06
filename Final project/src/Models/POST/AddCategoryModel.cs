using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Models.POST
{
    public class AddCategoryModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IFormFile CategoryImage { get; set; }
    }
}
