using Final_project.Entities;
using Final_project.Models.GET_models;

namespace Final_project.Models.General
{
    public class GetListCareerServiceResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public CareerListModel[]? Careers { get; set; }

        public int? TotalPages { get; set; }
    }
}
