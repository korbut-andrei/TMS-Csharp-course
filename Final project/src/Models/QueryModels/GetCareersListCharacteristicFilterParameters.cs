using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Models.QueryModels
{
    public class GetCareersListCharacteristicFilterParameters
    {
        [Required]
        public int CareerCharacteristicId { get; set; }
        public decimal? ValueFrom { get; set; }
        public decimal? ValueTo { get; set; }
        public string[]? StringValues { get; set; }
    }
}
