using Microsoft.EntityFrameworkCore;

namespace Lesson_17_Entity_Framework.Models
{
    public class ProfessionsContext : DbContext
    {
        public ProfessionsContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Professions> Professions { get; set;}
        public DbSet<Reviews> Reviews { get; set; }
    }
}
