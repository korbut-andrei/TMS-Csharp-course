using Final_project.Entities;

namespace Final_project.Models.General
{
    public class CareerServiceResponseModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public CareerEntity CareerEntity { get; set; }
    }
}
