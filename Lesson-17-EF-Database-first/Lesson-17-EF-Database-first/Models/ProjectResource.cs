using System;
using System.Collections.Generic;

namespace Lesson_17_EF_Database_first.Models
{
    public partial class ProjectResource
    {
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public int? RoleId { get; set; }

        public virtual ProjectRole? ProjectRole { get; set; }
        public virtual User? User { get; set; }
    }
}
