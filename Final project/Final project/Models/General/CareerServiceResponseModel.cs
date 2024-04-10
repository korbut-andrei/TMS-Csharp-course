using Final_project.Entities;

namespace Final_project.Models.General
{
    public class CareerServiceResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public CareerEntity? Career { get; set; }

    }
}
