using AndreiKorbut.CareerChoiceBackend.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Models.POST
{
    public class AddReviewWithBulletPointsModel
    {
        public string Text { get; set; }

        public decimal Rating { get; set; }
        public int UserId { get; set; }

        public int CareerId { get; set; }

        public List<AddReviewBulletPointModel>? ReviewBulletPoints { get; set; }
    }
}
