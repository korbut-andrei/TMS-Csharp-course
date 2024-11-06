using AndreiKorbut.CareerChoiceBackend.Enums;
using AndreiKorbut.CareerChoiceBackend.Models.CustomAttributes;
using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;
using AndreiKorbut.CareerChoiceBackend.Models.QueryModels;
using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Models.GETmodels
{
    public class GetCareersListQueryModel
    {
        [Required(ErrorMessage = "Page parameter can't be empty or 0.")]
        public int Page { get; set; }
        [Required(ErrorMessage = "RowsPerPage parameter can't be empty.")]
        [AllowedRowsPerPage(5,10,15,25,50, ErrorMessage = "You can only pick 5, 10, 15, 25, 50 items per page.")]
        public int RowsPerPage { get; set; }

        [Required(ErrorMessage = "Sorting parameter can't be empty.")]
        [EnumDataType(typeof(CareerListSortingOption), ErrorMessage = "Invalid sorting option.")]
        public CareerListSortingOption Sorting { get; set; }
        public GetCareersListCharacteristicFilterParameters[]? FilterParameters { get; set; }
        public int[]? CategoryIDs { get; set; }
        public AverageRatingRange? AverageRatingRange { get; set; }
        public SalaryFilterQuery? SalaryFilterQuery { get; set; }
        public EducationTimeRange? EducationTimeRange { get; set; }
    }
}