using System;
using System.Collections.Generic;

namespace Lesson_17_EF_Database_first.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = null!;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
