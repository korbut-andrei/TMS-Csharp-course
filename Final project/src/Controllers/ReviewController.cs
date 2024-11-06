using AndreiKorbut.CareerChoiceBackend.Models.POST;
using AndreiKorbut.CareerChoiceBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AndreiKorbut.CareerChoiceBackend.Controllers
{
    [Route("api/Reviews")]
    [ApiController]
    public class ReviewController
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Route("AddReview")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewWithBulletPointsModel addReviewWithBulletPointsModel)
        {
            var result = await _reviewService.AddReviewWithBulletPoints(addReviewWithBulletPointsModel);

            if (result.Success)
            {
                return new OkObjectResult(new
                {
                    Message = result.ServerMessage,
                    ReviewId = result.ReviewId,
                    BulletPoints = result.ReviewBulletPoints
                });
            }
            else
            {
                return new ObjectResult(new { error = result.ServerMessage })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
