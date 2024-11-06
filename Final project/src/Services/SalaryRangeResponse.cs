using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public class SalaryRangeResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public SalaryRange SalaryRange { get; set; }
    }
}

