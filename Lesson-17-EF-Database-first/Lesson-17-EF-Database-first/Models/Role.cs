using System;
using System.Collections.Generic;

namespace Lesson_17_EF_Database_first.Models
{
    public partial class Role
    {
        public Role()
        {
            ProjectRoles = new HashSet<ProjectRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ProjectRole> ProjectRoles { get; set; }
    }
}
