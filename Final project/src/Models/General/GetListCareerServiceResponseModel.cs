using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;

namespace AndreiKorbut.CareerChoiceBackend.Models.General
{
    public class GetListCareerServiceResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public CareerListModel[]? Careers { get; set; }

        public int? TotalPages { get; set; }
    }
}
