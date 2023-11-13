// AddItemViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lesson_12_Web_View.Models
{ 
    public class AddItem
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public bool HasRecliner { get; set; }
    }
}


