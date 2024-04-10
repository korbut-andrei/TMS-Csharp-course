using Final_project.Models.General;
using Final_project.Models.GETmodels;
using Final_project.Models.POST;

namespace Final_project.Services
{
    public interface IImageService
    {
        Task<ImageResponseModel> AddImage(IFormFile image);

        Task<ImageResponseModel> GetImage(int image);

        Task<ImageResponseModel> DeleteImage(int image);
    }
}
