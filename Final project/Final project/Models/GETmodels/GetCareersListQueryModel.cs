using Final_project.Models.GET_models;
using Final_project.Models.QueryModels;
using System.ComponentModel.DataAnnotations;

namespace Final_project.Models.GETmodels
{
    public class GetCareersListQueryModel
    {
        [Required]
        public int Page { get; set; }
        [Required]
        public int RowsPerPage { get; set; }
        [Required]
        public string Sorting { get; set; }
        public GetCareersListCharacteristicFilterParameters[]? FilterParameters { get; set; }
        public string[]? CategoryIDs { get; set; }
        public AverageRatingRange? AverageRatingRange { get; set; }
        public SalaryFilterQuery? SalaryFilterQuery { get; set; }
        public EducationTimeRange? EducationTimeRange { get; set; }
    }
}
