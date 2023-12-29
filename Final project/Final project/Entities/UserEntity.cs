using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Entities
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Base64ImageData { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
