namespace Final_project.Models.GET_models
{
    public class AverageSalaryByExperience
    {
        private decimal _averageSalary;

        public string ExperienceYears { get; set; }

        public decimal AverageSalary
        {
            get => _averageSalary;
            set => _averageSalary = Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }
    }
}
