using Final_project.Entities;

namespace Final_project.Models.GET_models
{
    public class CareerDetailsModel
    {
        public string ProfessionName { get; set; }
        public string Description { get; set; }
        public SalaryRange SalaryRange { get; set; }
        public string Category { get; set; }
        public string ImgURL { get; set; }
        public TypicalTaskList[] TypicalTasks { get; set; }
        public CareerCharacteristicEntity[] CareerCharacteristics { get; set; }

        public ReviewEntity[] Reviews { get; set; }

        public SalaryReportEntity[] SalaryStatistics { get; set; }

        public AverageReviewAndReviewCount AverageReviewAndReviewCount { get; set; }
    }
}
