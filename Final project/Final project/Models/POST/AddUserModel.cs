using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models.POST
{
    public class AddUserModel
    {
        public string Id { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? Country { get; set; }

    }
}
