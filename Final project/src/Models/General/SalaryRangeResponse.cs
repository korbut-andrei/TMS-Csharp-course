using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;

namespace CareerChoiceBackend.Models.General
{
    public class SalaryRangeResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public SalaryRange SalaryRange { get; set; }
    }
}

