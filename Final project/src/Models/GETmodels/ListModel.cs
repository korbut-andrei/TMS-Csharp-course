using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;
using AndreiKorbut.CareerChoiceBackend.Models.POST;

namespace AndreiKorbut.CareerChoiceBackend.Models.GETmodels
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
