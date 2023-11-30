using System;
using System.Collections.Generic;

namespace Lesson_17_EF_Database_first.Models
{
    public partial class ProjectRole
    {
        public int ProjectId { get; set; }
        public int RoleId { get; set; }
        public int? ProgrammingLanguageId { get; set; }
        public int PlannedResource { get; set; }
        public int? CurrentResource { get; set; }

        public virtual ProgrammingLanguage? ProgrammingLanguage { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
