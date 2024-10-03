using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Entities
{
    public class UserEntity : IdentityUser<int>
    {
        public int? ImageId { get; set; }
        public string? Country { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
