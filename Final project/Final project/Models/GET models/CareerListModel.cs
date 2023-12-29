namespace Final_project.Models.GET_models
{
    public class CareerListModel
    {
        public string CareerName { get; set; }
        public SalaryRange SalaryRange { get; set; }
        public string Category { get; set; }
        public string Base64ImageData { get; set; }
        public AverageReviewAndReviewCount AverageReviewAndReviewCount { get; set; }
    }
}
