using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Models.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "UserAgent is required")]
        public string UserAgent { get; set; } //data aboout client's software

    }
}
