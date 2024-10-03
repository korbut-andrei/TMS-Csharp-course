using Final_project.Entities.DbContexts;
using Final_project.Entities;
using Final_project.Models.GETmodels;
using Final_project.Models.POST;
using Final_project.Models.General;
using System.Diagnostics;
using System;
using Microsoft.AspNetCore.Identity;

namespace Final_project.Services
{
    public class ReviewService
    {
        private readonly CareerContext _dbContext;

        private readonly DbRecordsCheckService _dbRecordsCheckService;

        public ReviewService(CareerContext dbContext, DbRecordsCheckService dbRecordsCheckService)
        {
            _dbContext = dbContext;
            _dbRecordsCheckService = dbRecordsCheckService;
        }

        public async Task<ReviewResponseModel> AddReviewWithBulletPoints(AddReviewWithBulletPointsModel addReviewWithBulletPointsModel)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var reviewEntity = AddReview(new AddReviewModel
                {
                    CareerId = addReviewWithBulletPointsModel.CareerId,
                    Rating = addReviewWithBulletPointsModel.Rating,
                    Text = addReviewWithBulletPointsModel.Text,
                    UserId = addReviewWithBulletPointsModel.UserId
                });

                if (!reviewEntity.Success)
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"{reviewEntity.ServerMessage}"
                    };
                }

                var reviewBulletPointResponses = new List<AddReviewBulletPointResponseModel>();

                if (addReviewWithBulletPointsModel.ReviewBulletPoints != null && addReviewWithBulletPointsModel.ReviewBulletPoints.Any())
                {
                    foreach (var bulletPointModel in addReviewWithBulletPointsModel.ReviewBulletPoints)
                    {
                        var reviewBulletPointEntity = AddReviewBulletPoint(new AddReviewBulletPointDto
                        {
                            ReviewId = reviewEntity.ReviewId,
                            Type = bulletPointModel.Type,
                            Text = bulletPointModel.Text,
                        });

                        if (!reviewBulletPointEntity.Success)
                        {
                            return new ReviewResponseModel
                            {
                                Success = false,
                                ReviewId = 0,
                                ServerMessage = $"{reviewBulletPointEntity.ServerMessage}",
                                ReviewBulletPoints = reviewBulletPointResponses
                            };
                        }

                        var bulletPointResponse = reviewBulletPointEntity.ReviewBulletPoints.First();
                        reviewBulletPointResponses.Add(new AddReviewBulletPointResponseModel
                        {
                            Id = bulletPointResponse.Id,
                            Text = bulletPointResponse.Text,
                            Type = bulletPointResponse.Type,
                            ReviewId = bulletPointResponse.ReviewId
                        });


                        /*
                        reviewBulletPointResponses.Add(new AddReviewBulletPointResponseModel
                        {
                            Id = reviewBulletPointEntity.ReviewBulletPoints[0].Id,
                            Text = reviewBulletPointEntity.ReviewBulletPoints[0].Text,
                            Type = reviewBulletPointEntity.ReviewBulletPoints[0].Type,
                            ReviewId = reviewBulletPointEntity.ReviewBulletPoints[0].ReviewId
                        });
                        */
                    }
                }

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();


                return new ReviewResponseModel
                {
                    Success = true,
                    ReviewId = reviewEntity.ReviewId,
                    ServerMessage = $"Review has been successfully created.",
                    ReviewBulletPoints = reviewBulletPointResponses
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ReviewResponseModel
                {
                    Success = false,
                    ReviewId = 0,
                    ServerMessage = $"{ex.Message}."
                };
            }
        }

        private ReviewResponseModel AddReview(AddReviewModel addReviewModel)
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(addReviewModel.Text))
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"Review text can't be empty."
                    };
                }

                if (addReviewModel.Rating == null || addReviewModel.Rating == 0)
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"Review rating can't be empty or 0."
                    };
                }

                if (addReviewModel.CareerId == null || addReviewModel.CareerId == 0)
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"Career description can't be empty."
                    };
                }

                if (addReviewModel.UserId == null || addReviewModel.UserId == 0)
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"UserId can't be empty."
                    };
                }

                if (!_dbRecordsCheckService.RecordExistsInDatabase(addReviewModel.CareerId, "Careers", "Id"))
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"Provided careerId doesn't exist."
                    };
                }

                if (!_dbRecordsCheckService.RecordExistsInDatabase(addReviewModel.UserId, "UserRecords", "Id"))
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"Provided userId doesn't exist."
                    };
                }

                var reviewEntity = new ReviewEntity
                {
                    Rating = addReviewModel.Rating,
                    Text = addReviewModel.Text,
                    CareerId = addReviewModel.CareerId,
                    UserId = addReviewModel.UserId,
                    IsApproved = false,
                    IsDeleted = false,
                    DateTimeInUtc = DateTime.UtcNow
                };

                _dbContext.Reviews.Add(reviewEntity);
                _dbContext.SaveChanges();

                return new ReviewResponseModel
                {
                    Success = true,
                    ReviewId = reviewEntity.Id,
                    ServerMessage = $"Review has been created."
                };
            }
            catch (Exception ex)
            {
                return new ReviewResponseModel
                {
                    Success = false,
                    ReviewId = 0,
                    ServerMessage = $"{ex.Message}"
                };
            }
        }

        private ReviewResponseModel AddReviewBulletPoint(AddReviewBulletPointDto addReviewBulletPointModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addReviewBulletPointModel.Text))
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"Bullett point text can't be empty."
                    };
                }

                if (string.IsNullOrWhiteSpace(addReviewBulletPointModel.Type))
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"Bullett point type can't be empty."
                    };
                }

                if (addReviewBulletPointModel.ReviewId == null || addReviewBulletPointModel.ReviewId == 0)
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"ReviewId can't be empty or 0."
                    };
                }

                if (!_dbRecordsCheckService.RecordExistsInDatabase(addReviewBulletPointModel.ReviewId, "Reviews", "Id"))
                {
                    return new ReviewResponseModel
                    {
                        Success = false,
                        ReviewId = 0,
                        ServerMessage = $"Provided reviewId doesn't exist."
                    };
                }

                var reviewBulletPoint = new ReviewBulletPointEntity
                {
                    Text = addReviewBulletPointModel.Text,
                    Type = addReviewBulletPointModel.Type,
                    ReviewEntityId = addReviewBulletPointModel.ReviewId 
                };

                _dbContext.ReviewBulletPoints.Add(reviewBulletPoint);

                _dbContext.SaveChanges();


                return new ReviewResponseModel
                {
                    Success = true,
                    ReviewId = reviewBulletPoint.ReviewEntityId,
                    ServerMessage = $"Review bullet point has been successfully created.",
                    ReviewBulletPoints = new List<AddReviewBulletPointResponseModel> 
                    { new AddReviewBulletPointResponseModel
                        {
                            Text = addReviewBulletPointModel.Text,
                            Type = addReviewBulletPointModel.Type,
                            ReviewId = addReviewBulletPointModel.ReviewId,
                            Id = reviewBulletPoint.Id
                        } 
                    }
                };
            }
            catch (Exception ex)
            {
                return new ReviewResponseModel
                {
                    Success = false,
                    ReviewId = 0,
                    ServerMessage = $"{ex.Message}"
                };
            }
        }

    }
}
