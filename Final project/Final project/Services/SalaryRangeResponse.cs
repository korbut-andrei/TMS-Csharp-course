using Final_project.Entities;
using Final_project.Models.GET_models;

namespace Final_project.Services
{
    public class SalaryRangeResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public SalaryRange SalaryRange { get; set; }
    }
}

