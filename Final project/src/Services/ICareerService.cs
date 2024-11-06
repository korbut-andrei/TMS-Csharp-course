using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using AndreiKorbut.CareerChoiceBackend.Models.QueryModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public interface ICareerService
    {
        Task<CareerServiceResponseModel> AddCareer(AddCareerModel addCareerModel);

        Task<CareerServiceResponseModel> EditCareer(EditCareerModel editCareerModel);
        
        Task<GetDetailsCareerServiceResponseModel> GetCareer(int careerId);

        Task<CareerServiceResponseModel> DeleteCareer(int careerId);

        Task<GetListCareerServiceResponseModel> GetListOfCareers(
        [Required] GetCareersListQueryModel queryParameters
        );
    }
}
