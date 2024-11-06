using AndreiKorbut.CareerChoiceBackend.Models.POST;

namespace AndreiKorbut.CareerChoiceBackend.Models.GETmodels
{
    public class ReviewResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public int ReviewId { get; set; }
        public List<AddReviewBulletPointResponseModel>? ReviewBulletPoints { get; set; }
    }
}
