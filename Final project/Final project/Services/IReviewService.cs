using Final_project.Models.GETmodels;
using Final_project.Models.POST;

namespace Final_project.Services
{
    public interface IReviewService
    {
        Task<ReviewResponseModel> AddReviewWithBulletPoints(AddReviewWithBulletPointsModel addReviewWithBulletPointsModel);

    }
}
