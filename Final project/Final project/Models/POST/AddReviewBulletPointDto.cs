using Final_project.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_project.Models.POST
{
    public class AddReviewBulletPointDto
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public int ReviewId { get; set; }

    }
}