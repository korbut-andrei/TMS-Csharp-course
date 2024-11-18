using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Models.POST;

namespace CareerChoiceBackend.Interfaces
{
    public interface IImageService
    {
        Task<ImageResponseModel> AddImage(IFormFile image);

        Task<ImageResponseModel> GetImage(int image);

        Task<ImageResponseModel> DeleteImage(int image);
    }
}
