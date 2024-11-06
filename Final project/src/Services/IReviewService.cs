using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Models.POST;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public interface IReviewService
    {
        Task<ReviewResponseModel> AddReviewWithBulletPoints(AddReviewWithBulletPointsModel addReviewWithBulletPointsModel);

    }
}
