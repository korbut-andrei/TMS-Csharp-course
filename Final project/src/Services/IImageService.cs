using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Models.POST;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public interface IImageService
    {
        Task<ImageResponseModel> AddImage(IFormFile image);

        Task<ImageResponseModel> GetImage(int image);

        Task<ImageResponseModel> DeleteImage(int image);
    }
}
