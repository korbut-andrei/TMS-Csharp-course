using Final_project.Models.GET_models;

namespace Final_project.Services
{
    public class AverageReviewRatingResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public AverageReviewRatingAndReviewCount AverageReviewRatingAndReviewCount { get; set; }
    }
}
