using AndreiKorbut.CareerChoiceBackend.Entities;

namespace AndreiKorbut.CareerChoiceBackend.Models.General
{
    public class CareerServiceResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public CareerEntity? Career { get; set; }

    }
}
