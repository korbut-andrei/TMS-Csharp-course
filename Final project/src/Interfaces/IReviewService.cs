using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using AndreiKorbut.CareerChoiceBackend.Services;
using Microsoft.EntityFrameworkCore;

namespace CareerChoiceBackend.Interfaces
{
    public interface IReviewService
    {
        public Task<ReviewResponseModel> AddReviewWithBulletPoints(AddReviewWithBulletPointsModel addReviewWithBulletPointsModel);
    }
}
