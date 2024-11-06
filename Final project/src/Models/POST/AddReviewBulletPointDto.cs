using AndreiKorbut.CareerChoiceBackend.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Models.POST
{
    public class AddReviewBulletPointDto
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public int ReviewId { get; set; }

    }
}