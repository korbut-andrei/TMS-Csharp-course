using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lesson_17_Entity_Framework.Entities
{
    public class IdentityContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
