using AndreiKorbut.CareerChoiceBackend.Models.GET_models;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public class AverageReviewRatingResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public AverageReviewRatingAndReviewCount AverageReviewRatingAndReviewCount { get; set; }
    }
}
