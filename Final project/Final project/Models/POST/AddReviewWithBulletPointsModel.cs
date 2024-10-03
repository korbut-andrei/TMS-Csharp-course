using Final_project.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Final_project.Models.POST
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
