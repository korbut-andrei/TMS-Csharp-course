using AndreiKorbut.CareerChoiceBackend.Models.GET_models;

namespace CareerChoiceBackend.Models.General
{
    public class AverageReviewRatingResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public AverageReviewRatingAndReviewCount AverageReviewRatingAndReviewCount { get; set; }
    }
}
