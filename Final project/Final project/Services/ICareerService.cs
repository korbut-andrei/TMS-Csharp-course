using Final_project.Entities;
using Final_project.Models.General;
using Final_project.Models.POST;
using Microsoft.AspNetCore.Mvc;

namespace Final_project.Services
{
    public interface ICareerService
    {
        Task<CareerServiceResponseModel> AddCareer(AddCareerModel addCareerModel);

        Task<CareerServiceResponseModel> EditCareer(EditCareerModel editCareerModel);
        
        Task<GetDetailsCareerServiceResponseModel> GetCareer(int careerId);

        Task<CareerServiceResponseModel> DeleteCareer(int careerId);


    }
}
