using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;

namespace AndreiKorbut.CareerChoiceBackend.Models.General
{
    public class GetDetailsCareerServiceResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public CareerDetailsModel? Career { get; set; }
    }
}
