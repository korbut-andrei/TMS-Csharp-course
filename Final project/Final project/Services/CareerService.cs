using Final_project.Entities;
using Final_project.Entities.DbContexts;
using Final_project.Models.General;
using Final_project.Models.GET_models;
using Final_project.Models.GETmodels;
using Final_project.Models.POST;
using Final_project.Models.QueryModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_project.Services
{
    public class CareerService : ICareerService
    {
        public List<CareerEntity> Careers { get; } = new List<CareerEntity>();

        private readonly CareerContext _dbContext;

        private readonly ImageService _imageService;

        private readonly DbRecordsCheckService _dbRecordsCheckService;

        public CareerService(CareerContext dbContext, ImageService imageService, DbRecordsCheckService dbRecordsCheckService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _dbRecordsCheckService = dbRecordsCheckService;
        }

        public async Task<CareerServiceResponseModel> AddCareer(AddCareerModel addCareerModel)
        {
            try
            {
                if (addCareerModel.CareerImage == null || addCareerModel.CareerImage.Length == 0)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"You need to upload a career image."
                    };
                }

                if (string.IsNullOrWhiteSpace(addCareerModel.Name))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career name can't be empty."
                    };
                }
                if (string.IsNullOrWhiteSpace(addCareerModel.Description))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career description can't be empty."
                    };
                }
                if (addCareerModel.CategoryId == 0)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career categoryId can't be empty."
                    };
                }
                if (_dbRecordsCheckService.RecordExistsInDatabase(addCareerModel.CategoryId, "Categories", "Id"))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Provided career categoryId doesn't exist."
                    };
                }
                if (_dbRecordsCheckService.RecordExistsInDatabase(addCareerModel.Name, "Careers", "Name"))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Careeer with such name already exists."
                    };
                }

                var imageData = await _imageService.AddImage(addCareerModel.CareerImage);

                if (!imageData.Success)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = imageData.ServerMessage
                    };
                }

                var careerDto = new CareerDto
                {
                    Name = addCareerModel.Name,
                    Description = addCareerModel.Description,
                    CategoryId = addCareerModel.CategoryId,
                    ImageId = imageData.ImageId
                };

                var careerEntity = new CareerEntity
                {
                    Name = careerDto.Name,
                    Description = careerDto.Description,
                    CategoryId = careerDto.CategoryId,
                    ImageId = careerDto.ImageId,
                    IsDeleted = false
                };

                _dbContext.Careers.Add(careerEntity);
                await _dbContext.SaveChangesAsync();

                // Process other fields in careerAddModel

                return new CareerServiceResponseModel
                {
                    Success = true,
                    Career = careerEntity,
                    ServerMessage = $"Career has been successfully created."
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in AddCareer: {ex.Message}");
                return new CareerServiceResponseModel
                {
                    Success = false,
                    Career = null,
                    ServerMessage = $"Internal server error: {ex.Message}"
                };
            }
        }

        public async Task<CareerServiceResponseModel> EditCareer(EditCareerModel editCareerModel)
        {
            try
            {
                // Check if the provided careerId is valid
                var existingCareer = await _dbContext.Careers.FindAsync(editCareerModel.Id);
                if (existingCareer == null)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career with id {editCareerModel.Id} not found."
                    };
                }


                if (editCareerModel.CareerImage != null || editCareerModel.CareerImage.Length > 0)
                {
                    var imageData = await _imageService.AddImage(editCareerModel.CareerImage);

                    if (!imageData.Success)
                    {
                        // Handle the error condition
                        // For example, return an error response indicating failure
                        return new CareerServiceResponseModel
                        {
                            Success = false,
                            Career = null,
                            ServerMessage = imageData.ServerMessage
                        };
                    }

                    existingCareer.ImageId = imageData.ImageId;
                }

                // Update other properties if provided
                if (!string.IsNullOrWhiteSpace(editCareerModel.Name))
                {
                    if (_dbRecordsCheckService.RecordExistsInDatabase(editCareerModel.Name, "Careers", "Name"))
                    {
                        return new CareerServiceResponseModel
                        {
                            Success = false,
                            Career = null,
                            ServerMessage = $"Careeer with such name already exists."
                        };
                    }

                    existingCareer.Name = editCareerModel.Name;
                }

                if (!string.IsNullOrWhiteSpace(editCareerModel.Description))
                {
                    existingCareer.Description = editCareerModel.Description;
                }

                if (editCareerModel.CategoryId.HasValue)
                {
                    if (_dbRecordsCheckService.RecordExistsInDatabase(editCareerModel.CategoryId, "Categories", "Id"))
                    {
                        return new CareerServiceResponseModel
                        {
                            Success = false,
                            Career = null,
                            ServerMessage = $"Provided career categoryId doesn't exist."
                        };
                    }

                    if (editCareerModel.CategoryId == 0)
                    {
                        return new CareerServiceResponseModel
                        {
                            Success = false,
                            Career = null,
                            ServerMessage = $"Career categoryId can't be 0."
                        };
                    }

                    if (_dbRecordsCheckService.RecordExistsInDatabase(editCareerModel.Name, "Categories", "Name"))
                    {
                        return new CareerServiceResponseModel
                        {
                            Success = false,
                            Career = null,
                            ServerMessage = $"Provided category doesn't exist."
                        };
                    }

                    existingCareer.CategoryId = editCareerModel.CategoryId.Value;
                }

                await _dbContext.SaveChangesAsync();


                // Process other fields in careerAddModel

                return new CareerServiceResponseModel
                {
                    Success = true,
                    Career = existingCareer,
                    ServerMessage = $"Career has been edited successfully."
                };
            }
            catch (Exception ex)
            {
                return new CareerServiceResponseModel
                {
                    Success = false,
                    Career = null,
                    ServerMessage = $"Internal server error: {ex.Message}"
                };
            }
        }

        public async Task<GetDetailsCareerServiceResponseModel> GetCareer(int careerId)
        {
            try
            {
                // Check if the provided careerId is valid
                var existingCareer = await _dbContext.Careers.FindAsync(careerId);

                if (existingCareer == null)
                {
                    return new GetDetailsCareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career with id {careerId} not found."
                    };
                }
                if (existingCareer.IsDeleted)
                {
                    return new GetDetailsCareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"This career is no longer available."
                    };
                }

                var imageData = await _imageService.GetImage(existingCareer.ImageId);

                if (!imageData.Success)
                {
                    // Handle the error condition
                    // For example, return an error response indicating failure
                    return new GetDetailsCareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Server couldn't load the career image. Error: {imageData.ServerMessage}"
                    };
                }

                var careerCategory = await _dbContext.Categories.FindAsync(existingCareer.CategoryId);

                var salaryReports = await _dbContext.SalaryReports.Where(c => c.CareerId == existingCareer.Id).ToArrayAsync();

                int minSalary = salaryReports.Any() ? salaryReports.Min(s => s.Salary) : 0;

                int maxSalary = salaryReports.Any() ? salaryReports.Max(s => s.Salary) : 0;

                var typicalTaskEntityList = await _dbContext.TypicalTasks.Where(c => c.CareerId == existingCareer.Id).ToArrayAsync();

                var typicalTaskList =
                    typicalTaskEntityList.Select(tt => new TypicalTaskList
                    {
                        Id = tt.Id,
                        Description = tt.Description
                    }).ToArray();

                var careerCharacteristics = await _dbContext.Characteristics.ToArrayAsync();

                var reviewsWithBulletPoints = await _dbContext.Reviews.Include(r => r.ReviewBulletPoints).Where(r => r.CareerId == existingCareer.Id).ToArrayAsync();

                decimal averageReview = reviewsWithBulletPoints.Any() ? reviewsWithBulletPoints.Average(r => r.Rating) : 0.0m;

                // Get the count of reviews
                int reviewCount = reviewsWithBulletPoints.Length;

                // Example: Save to database
                var careerDetailsModel = new CareerDetailsModel
                {
                    Id = existingCareer.Id,
                    ProfessionName = existingCareer.Name,
                    Description = existingCareer.Description,
                    CareerImage = imageData.Image,
                    categoryName = careerCategory.Name,
                    SalaryRange = new SalaryRange { SalaryMin = minSalary, SalaryMax = maxSalary },
                    TypicalTasks = typicalTaskList,
                    CareerCharacteristics = careerCharacteristics,
                    Reviews = reviewsWithBulletPoints,
                    SalaryStatistics = salaryReports,
                    AverageReviewAndReviewCount = new AverageReviewRatingAndReviewCount { AverageReviewRating = averageReview, ReviewCount = reviewCount }
                };

                // Process other fields in careerAddModel

                return new GetDetailsCareerServiceResponseModel
                {
                    Success = true,
                    Career = careerDetailsModel,
                    ServerMessage = $"Career {careerId} has been successfully found."
                };

            }
            catch (Exception ex)
            {
                return new GetDetailsCareerServiceResponseModel
                {
                    Success = false,
                    Career = null,
                    ServerMessage = $"Internal server error: {ex.Message}"
                };
            }
        }


        public async Task<CareerServiceResponseModel> DeleteCareer(int careerId)
        {
            try
            {
                // Check if the provided careerId is valid
                var requestedCareer = await _dbContext.Careers.FindAsync(careerId);

                if (requestedCareer == null || requestedCareer.IsDeleted)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career with id {careerId} not found."
                    };
                }

                var imageDeletion = await _imageService.DeleteImage(requestedCareer.ImageId);

                if (!imageDeletion.Success)
                {
                    // Handle the error condition
                    // For example, return an error response indicating failure
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Deletion of career failed during the career image deletion. Error: {imageDeletion.ServerMessage}"
                    };
                }

                await _dbContext.SaveChangesAsync();

                return new CareerServiceResponseModel
                {
                    Success = true,
                    Career = null,
                    ServerMessage = $"Career {careerId} has been successfully deleted."
                };

            }
            catch (Exception ex)
            {
                return new CareerServiceResponseModel
                {
                    Success = false,
                    Career = null,
                    ServerMessage = $"Internal server error: {ex.Message}"
                };
            }
        }

        public async Task<GetListCareerServiceResponseModel> GetListOfCareers(
        [Required] GetCareersListQueryModel queryParameters
        )
        {
            IQueryable<CareerEntity> careerListToBeReturned = _dbContext.Careers.AsQueryable();

            try
            {
                if (queryParameters.FilterParameters != null || queryParameters.CategoryIDs != null
                    || queryParameters.AverageRatingRange != null || queryParameters.SalaryFilterQuery != null
                    || queryParameters.EducationTimeRange != null)
                {
                    if (string.IsNullOrEmpty(queryParameters.Sorting))
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"Sorting parameter can't be empty."
                        };
                    }

                    if (queryParameters.Page <= 0)
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"Page parameter can't be empty or 0."
                        };
                    }

                    if (queryParameters.RowsPerPage <= 0)
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"RowsPerPage parameter can't be empty."
                        };
                    }

                    int[] allowedRowsPerPage = { 5, 10, 15, 25, 50 };


                    if (!allowedRowsPerPage.Contains(queryParameters.RowsPerPage))
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"You can only pick 5, 10, 15, 25, 50 items per page."
                        };
                    }

                    if (queryParameters.FilterParameters != null)
                    {
                        foreach (var parameter in queryParameters.FilterParameters)
                        {
                            var characteristic = await _dbContext.Characteristics.FindAsync(parameter.CareerCharacteristicId);

                            if (characteristic == null)
                            {
                                return new GetListCareerServiceResponseModel
                                {
                                    Success = false,
                                    Careers = null,
                                    ServerMessage = $"Filtering career characteristic with Id {parameter.CareerCharacteristicId} not found."
                                };
                            }

                            if (characteristic.Type == "int")
                            {
                                // Update IQueryable with filter based on integer type
                                careerListToBeReturned = careerListToBeReturned.Where(c => _dbContext.CharacteristicReviews
                                                  .Where(ccr => ccr.CareerCharacteristicId == parameter.CareerCharacteristicId && ccr.IsApproved)
                                                  .GroupBy(ccr => ccr.CareerId)
                                                  .Select(group => group.Any() ? group.Average(ccr => ccr.Rating) : (decimal?)0m) // Handle cases where there are no ratings
                                                  .FirstOrDefault() >= parameter.ValueFrom &&
                                                _dbContext.CharacteristicReviews
                                                  .Where(ccr => ccr.CareerCharacteristicId == parameter.CareerCharacteristicId && ccr.IsApproved)
                                                  .GroupBy(ccr => ccr.CareerId)
                                                  .Select(group => group.Any() ? group.Average(ccr => ccr.Rating) : (decimal?)0m) // Handle cases where there are no ratings
                                                  .FirstOrDefault() <= parameter.ValueTo);

                                if (!careerListToBeReturned.Any())
                                {
                                    return new GetListCareerServiceResponseModel
                                    {
                                        Success = true,
                                        Careers = null,
                                        ServerMessage = $"There are no careers matching filtering criteria."
                                    };
                                }
                            }

                            if (characteristic.Type == "string")
                            {
                                if (characteristic.Type == "string")
                                {
                                    var mostCommonRatingStringsQuery = _dbContext.CharacteristicReviews
                                        .Where(ccr => ccr.CareerCharacteristicId == parameter.CareerCharacteristicId && ccr.IsApproved)
                                        .GroupBy(ccr => new { ccr.CareerId, ccr.RatingString })
                                        .Select(g => new
                                        {
                                            CareerId = g.Key.CareerId,
                                            RatingString = g.Key.RatingString,
                                            Count = g.Count()
                                        })
                                        .GroupBy(result => result.CareerId)
                                        .Select(group => new
                                        {
                                            CareerId = group.Key,
                                            MostCommonRatingString = group.OrderByDescending(x => x.Count).FirstOrDefault() != null
                                                ? group.OrderByDescending(x => x.Count).First().RatingString
                                                : "Not reviewed"
                                        });

                                    // Apply filtering based on string values
                                    var filteredCareerIds = mostCommonRatingStringsQuery
                                        .Where(result => parameter.StringValues.Any(str => str == result.MostCommonRatingString))
                                        .Select(result => result.CareerId)
                                        .ToList();

                                    // Filter the career list using the filteredCareerIds
                                    careerListToBeReturned = careerListToBeReturned.Where(c => filteredCareerIds.Contains(c.Id));
                                }

                                // Continue with other filterings and operations using careerListToBeReturned


                                if (!careerListToBeReturned.Any())
                                {
                                    return new GetListCareerServiceResponseModel
                                    {
                                        Success = true,
                                        Careers = null,
                                        ServerMessage = $"There are no careers matching filtering criteria."
                                    };
                                }
                            }
                        }
                    }

                    if (queryParameters.CategoryIDs != null && queryParameters.CategoryIDs.Length > 0)
                    {
                        careerListToBeReturned = careerListToBeReturned
                            .Where(career => queryParameters.CategoryIDs.Contains(career.CategoryId.ToString()));

                        if (!careerListToBeReturned.Any())
                        {
                            return new GetListCareerServiceResponseModel
                            {
                                Success = false,
                                Careers = null,
                                ServerMessage = $"There are no careers matching filtering criteria."
                            };
                        }
                    }

                    if (queryParameters.AverageRatingRange != null)
                    {

                        // Apply filtering based on average rating range directly to careerListToBeReturned
                        careerListToBeReturned = careerListToBeReturned
                            .GroupJoin(_dbContext.Reviews.Where(review => review.IsApproved),
                                career => career.Id,
                                review => review.CareerId,
                                (career, reviews) => new
                                {
                                    Career = career,
                                    Reviews = reviews
                                })
                            .Select(joinedCareer => new
                            {
                                Career = joinedCareer.Career,
                                AverageRating = joinedCareer.Reviews.Any() ? joinedCareer.Reviews.Average(review => review.Rating) : 0m
                            })
                            .Where(result => result.AverageRating >= queryParameters.AverageRatingRange.RatingFrom &&
                                             result.AverageRating <= queryParameters.AverageRatingRange.RatingTo)
                            .Select(result => result.Career);

                        if (!careerListToBeReturned.Any())
                        {
                            return new GetListCareerServiceResponseModel
                            {
                                Success = false,
                                Careers = null,
                                ServerMessage = $"There are no careers matching filtering criteria."
                            };
                        }
                    }

                    if (queryParameters.SalaryFilterQuery != null)
                    {
                        careerListToBeReturned = careerListToBeReturned
                            .GroupJoin(_dbContext.SalaryReports.Where(salary => salary.IsApproved),
                                career => career.Id,
                                salaryReport => salaryReport.CareerId,
                                (career, salaryReports) => new
                                {
                                    Career = career,
                                    SalaryReports = salaryReports
                                })
                            .Select(joinedCareer => new
                            {
                                Career = joinedCareer.Career,
                                AverageSalary = joinedCareer.SalaryReports
                                .Where(sr => sr.ExperienceYears == queryParameters.SalaryFilterQuery.ExperienceYears)
                                .Select(sr => sr.Salary)
                                .DefaultIfEmpty(0)
                                .Average()
                            })
                            .Where(result =>
                                result.AverageSalary >= queryParameters.SalaryFilterQuery.AverageSalaryMin &&
                                result.AverageSalary <= queryParameters.SalaryFilterQuery.AverageSalaryMax)
                            .Select(result => result.Career);

                        if (!careerListToBeReturned.Any())
                        {
                            return new GetListCareerServiceResponseModel
                            {
                                Success = false,
                                Careers = null,
                                ServerMessage = $"There are no careers matching filtering criteria."
                            };
                        }
                    }

                    if (queryParameters.EducationTimeRange != null)
                    {
                        // Filter careers based on education options
                        careerListToBeReturned = careerListToBeReturned
                            .GroupJoin(_dbContext.EducationOptions,
                                career => career.Id,
                                educationOption => educationOption.CareerId,
                                (career, educationOptions) => new
                                {
                                    Career = career,
                                    EducationOptions = educationOptions
                                })
                            .Select(joinedCareer =>
                                new
                                {
                                    Career = joinedCareer.Career,
                                    AverageEducationTime = joinedCareer.EducationOptions.Any()
                                        ? joinedCareer.EducationOptions.Average(eo => eo.TimeLength)
                                        : 0
                                })
                            .Where(result =>
                                result.AverageEducationTime >= queryParameters.EducationTimeRange.EducationTimeMin &&
                                result.AverageEducationTime <= queryParameters.EducationTimeRange.EducationTimeMax)
                            .Select(result => result.Career);

                        if (!careerListToBeReturned.Any())
                        {
                            return new GetListCareerServiceResponseModel
                            {
                                Success = false,
                                Careers = null,
                                ServerMessage = $"There are no careers matching filtering criteria."
                            };
                        }
                    }

                    var sortingResult = await SortList(careerListToBeReturned, queryParameters.Sorting);
                    if (sortingResult.Success != true)
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"{sortingResult.ServerMessage}"
                        };
                    }

                    var paginationResult = await PaginateList(careerListToBeReturned, queryParameters.Page, queryParameters.RowsPerPage);
                    if (paginationResult.Success != true)
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"{paginationResult.ServerMessage}"
                        };
                    }

                    var mappedResult = await MapCareersIntoCareerListModel(paginationResult.Careers);
                    if (paginationResult.Success != true)
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"{paginationResult.ServerMessage}"
                        };
                    }

                    return new GetListCareerServiceResponseModel
                    {
                        Careers = mappedResult.MappedCareers,
                        ServerMessage = $"List of careers successfully collected.",
                        Success = true,
                        TotalPages = paginationResult.TotalPages
                    };
                }

                else
                {
                    var sortingResult = await SortList(careerListToBeReturned, queryParameters.Sorting);
                    if (sortingResult.Success != true)
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"{sortingResult.ServerMessage}"
                        };
                    }

                    var paginationResult = await PaginateList(sortingResult.Careers, queryParameters.Page, queryParameters.RowsPerPage);
                    if (paginationResult.Success != true)
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"{paginationResult.ServerMessage}"
                        };
                    }

                    var mappedResult = await MapCareersIntoCareerListModel(paginationResult.Careers);
                    if (paginationResult.Success != true)
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"{paginationResult.ServerMessage}"
                        };
                    }

                    return new GetListCareerServiceResponseModel
                    {
                        Careers = mappedResult.MappedCareers,
                        ServerMessage = $"List of careers successfully collected.",
                        Success = true,
                        TotalPages = paginationResult.TotalPages
                    };
                }
            }
            catch (Exception ex)
            {
                return new GetListCareerServiceResponseModel
                {
                    Careers = null,
                    ServerMessage = $"List of careers collection failed: {ex.Message}.",
                    Success = false,
                    TotalPages = 0
                };
            }
        }

        private async Task<ListModel> SortList(IQueryable<CareerEntity> careerListToBeSorted, string sorting)
        {
            try
            {
                switch (sorting)
                {
                    case "By recommended":
                        // Implement random sorting logic for "By recommended"
                        Random random = new Random();
                        careerListToBeSorted = careerListToBeSorted.OrderBy(c => random.Next());
                        break;

                    case "By rating asc":
                        careerListToBeSorted = careerListToBeSorted.OrderBy(c => _dbContext.Reviews
                            .Where(review => review.IsApproved && review.CareerId == c.Id)
                            .Select(review => review.Rating)
                            .DefaultIfEmpty(0)
                            .Average());
                        break;

                    case "By rating desc":
                        careerListToBeSorted = careerListToBeSorted.OrderByDescending(c => _dbContext.Reviews
                            .Where(review => review.IsApproved && review.CareerId == c.Id)
                            .Select(review => review.Rating)
                            .DefaultIfEmpty(0)
                            .Average());
                        break;

                    case "By average wages asc":
                        careerListToBeSorted = careerListToBeSorted.OrderBy(c => _dbContext.SalaryReports
                            .Where(salary => salary.CareerId == c.Id && salary.IsApproved)
                            .Select(salary => salary.Salary)
                            .DefaultIfEmpty(0)
                            .Average());
                        break;

                    case "By average wages desc":
                        careerListToBeSorted = careerListToBeSorted.OrderByDescending(c => _dbContext.SalaryReports
                            .Where(salary => salary.CareerId == c.Id && salary.IsApproved)
                            .Select(salary => salary.Salary)
                            .DefaultIfEmpty(0)
                            .Average());
                        break;

                    default:
                        // If an invalid sorting parameter is applied, return an error response
                        return new ListModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = "Unknown error during sorting."
                        };
                }

                // Return sorted careers
                return new ListModel
                {
                    Success = true,
                    Careers = careerListToBeSorted,
                    ServerMessage = "List has been successfully sorted."
                };
            }
            catch (Exception ex)
            {
                return new ListModel
                {
                    Success = false,
                    Careers = null,
                    ServerMessage = $"Internal server error: Sorting failed. {ex.Message}"
                };
            }
        }

        private async Task<ListModel> PaginateList(IQueryable<CareerEntity> careerListToBePaged, int page, int rowsPerPage)
        {
            try
            {
                int totalItemCount = careerListToBePaged.Count();

                int totalPages = (int)Math.Ceiling((double)totalItemCount / rowsPerPage);

                careerListToBePaged = careerListToBePaged
                    .Skip((page - 1) * rowsPerPage)
                    .Take(rowsPerPage);

                return new ListModel
                {
                    Success = true,
                    Careers = careerListToBePaged,
                    TotalPages = totalPages,
                    ServerMessage = "List has been successfully paged."
                };
            }
            catch (Exception ex)
            {
                return new ListModel
                {
                    Success = false,
                    Careers = null,
                    ServerMessage = $"Internal server error: Pagination failed. {ex.Message}"
                };
            }
        }

        private async Task<ListModel> MapCareersIntoCareerListModel(IQueryable<CareerEntity> careerListToBeMapped)
        {
            try
            {
                var mappedCareers = new List<CareerListModel>();

                foreach (var career in careerListToBeMapped)
                {
                    var careerListModel = new CareerListModel
                    {
                        Id = career.Id,
                        Name = career.Name,
                        CategoryName = await _dbContext.Categories
                            .Where(cat => cat.Id == career.CategoryId)
                            .Select(cat => cat.Name)
                            .FirstOrDefaultAsync()
                    };

                    var salaryRangeResponse = await GetSalaryRangeForCareer(career.Id);

                    if (!salaryRangeResponse.Success)
                    {
                        return new ListModel
                        {
                            Success = false,
                            MappedCareers = null,
                            ServerMessage = $"Salary range mapping failed: {salaryRangeResponse.ServerMessage}."
                        };
                    }

                    var averageReviewResponse = await GetAverageReviewAndReviewCount(career.Id);

                    if (!averageReviewResponse.Success)
                    {
                        return new ListModel
                        {
                            Success = false,
                            MappedCareers = null,
                            ServerMessage = $"Image mapping failed: {averageReviewResponse.ServerMessage}."
                        };
                    }

                    var imageResponse = await _imageService.GetImage(career.ImageId);

                    if (!imageResponse.Success)
                    {
                        return new ListModel
                        {
                            Success = false,
                            MappedCareers = null,
                            ServerMessage = $"Image mapping failed: {imageResponse.ServerMessage}."
                        };
                    }

                    careerListModel.CareerImage = imageResponse.Image;

                    mappedCareers.Add(careerListModel);
                }

                return new ListModel
                {
                    Success = true,
                    MappedCareers = mappedCareers.ToArray(),
                    ServerMessage = "List has been successfully mapped."
                };
            }
            catch (Exception ex)
            {
                return new ListModel
                {
                    Success = false,
                    MappedCareers = null,
                    ServerMessage = $"Internal server error: Mapping failed. {ex.Message}"
                };
            }
        }

        private async Task<SalaryRangeResponse> GetSalaryRangeForCareer(int careerId)
        {
            try
            {
                var salaryReports = _dbContext.SalaryReports
                    .Where(sr => sr.CareerId == careerId && sr.IsApproved)
                    .Select(sr => sr.Salary)
                    .ToList();

                return new SalaryRangeResponse
                {
                    Success = true,
                    SalaryRange = new SalaryRange
                    {
                        SalaryMin = salaryReports.Any() ? salaryReports.Min() : 0,
                        SalaryMax = salaryReports.Any() ? salaryReports.Max() : 0
                    },
                    ServerMessage = salaryReports.Any() ? "Salary range has been successfully calculated." : "No salary reports found for the career."
                };
            }
            catch (Exception ex)
            {
                return new SalaryRangeResponse
                {
                    Success = false,
                    SalaryRange = null,
                    ServerMessage = $"Salary range calculation failed: {ex.Message}."
                };
            }
        }

        // Helper method to get average review rating and review count
        private async Task<AverageReviewRatingResponse> GetAverageReviewAndReviewCount(int careerId)
        {
            try
            {
                var reviews = _dbContext.Reviews
                .Where(review => review.IsApproved && review.CareerId == careerId)
                .ToList();

                decimal averageRating = reviews.Any() ? reviews.Average(review => review.Rating) : 0m;
                int reviewCount = reviews.Count;

                return new AverageReviewRatingResponse
                {
                    Success = false,
                    AverageReviewRatingAndReviewCount = new AverageReviewRatingAndReviewCount
                    {
                        AverageReviewRating = averageRating,
                        ReviewCount = reviewCount
                    },
                    ServerMessage = $"Average review and review count successfully calcualted."
                };
            }
            catch (Exception ex)
            {
                return new AverageReviewRatingResponse
                {
                    Success = false,
                    AverageReviewRatingAndReviewCount = null,
                    ServerMessage = $"Average review and review count calculation failed: {ex.Message}."
                };
            }
        }
    }
}