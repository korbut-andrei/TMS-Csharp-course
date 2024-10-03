using Final_project.Entities;
using Final_project.Models.GET_models;
using Final_project.Models.POST;

namespace Final_project.Models.GETmodels
{
    public class ListModel
    {
        public bool Success { get; set; }
        public IQueryable<CareerDto>? Careers { get; set; }
        public string ServerMessage { get; set; }
        public CareerListModel[]? MappedCareers { get; set; }

        public int? TotalPages { get; set; }
    }
}
