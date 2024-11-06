using AndreiKorbut.CareerChoiceBackend.Entities;

namespace AndreiKorbut.CareerChoiceBackend.Models.GET_models
{
    public class CareerDetailsModel
    {
        public int Id { get; set; }
        public string ProfessionName { get; set; }
        public string Description { get; set; }
        public SalaryRange SalaryRange { get; set; }
        public string categoryName { get; set; }
        public IFormFile CareerImage { get; set; }
        public TypicalTaskList[] TypicalTasks { get; set; }
        public CareerCharacteristicEntity[] CareerCharacteristics { get; set; }

        public ReviewEntity[] Reviews { get; set; }

        public SalaryReportEntity[] SalaryStatistics { get; set; }

        public AverageReviewRatingAndReviewCount AverageReviewAndReviewCount { get; set; }
    }
}
