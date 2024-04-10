using Final_project.Entities;
using Final_project.Models.General;
using Final_project.Models.GET_models;
using Final_project.Models.GETmodels;
using Final_project.Models.POST;
using Final_project.Models.QueryModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Final_project.Services
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
