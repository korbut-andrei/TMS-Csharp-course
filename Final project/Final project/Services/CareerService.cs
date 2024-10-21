using Final_project.Entities;
using Final_project.Entities.DbContexts;
using Final_project.Enums;
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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
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
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_project.Services
{
    public class CareerService : ICareerService
    {

        private readonly IMemoryCache _cache;

        private readonly CareerContext _dbContext;

        private readonly ImageService _imageService;

        private readonly HashHelper _hashHelper;

        private readonly DbRecordsCheckService _dbRecordsCheckService;

        private readonly ConcurrentBag<string> _careerCacheKeys;

        private readonly ILogger<CareerService> _logger;


        public CareerService(CareerContext dbContext, ImageService imageService, 
            DbRecordsCheckService dbRecordsCheckService, HashHelper hashHelper,
            IMemoryCache memoryCache, ILogger<CareerService> logger)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _dbRecordsCheckService = dbRecordsCheckService;
            _hashHelper = hashHelper;
            _cache = memoryCache;
            _logger = logger;
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
                if (!_dbRecordsCheckService.RecordExistsInDatabase(addCareerModel.CategoryId, "Categories", "Id"))
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

                var careerEntity = new CareerEntity
                {
                    Name = addCareerModel.Name,
                    Description = addCareerModel.Description,
                    CategoryId = addCareerModel.CategoryId,
                    ImageId = imageData.ImageId,
                    IsDeleted = false
                };

                _dbContext.Careers.Add(careerEntity);
                await _dbContext.SaveChangesAsync();

                ClearCache();

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
                        return new CareerServiceResponseModel
                        {
                            Success = false,
                            Career = null,
                            ServerMessage = imageData.ServerMessage
                        };
                    }

                    existingCareer.ImageId = imageData.ImageId;
                }

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

                ClearCache();

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

                var cacheKey = $"career_id_{careerId}";

                if (!_cache.TryGetValue(cacheKey, out GetDetailsCareerServiceResponseModel cachedCareerResponseModel))
                {
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

                    int reviewCount = reviewsWithBulletPoints.Length;

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

                    var careerResponseModel =  new GetDetailsCareerServiceResponseModel
                    {
                        Success = true,
                        Career = careerDetailsModel,
                        ServerMessage = $"Career {careerId} has been successfully found."
                    };

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    };


                    _careerCacheKeys.Add(cacheKey.ToString());
                    _cache.Set(cacheKey, careerResponseModel, cacheOptions);

                    return careerResponseModel;
                }

                return cachedCareerResponseModel;

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
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Deletion of career failed during the career image deletion. Error: {imageDeletion.ServerMessage}"
                    };
                }

                await _dbContext.SaveChangesAsync();

                ClearCache();

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

        public async Task<GetListCareerServiceResponseModel> GetListOfCareers(GetCareersListQueryModel queryParameters)
        {
            _logger.LogInformation("GetListOfCareers service method started.");

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(queryParameters);

            if (!Validator.TryValidateObject(queryParameters, validationContext, validationResults, true))
            {
                var errorMessages = validationResults.Select(v => v.ErrorMessage).ToList();
                _logger.LogError("Validation errors: {ValidationErrors}", string.Join(", ", errorMessages));
                return new GetListCareerServiceResponseModel
                {
                    Success = false,
                    Careers = null,
                    ServerMessage = string.Join("; ", errorMessages)
                };
            }

            try
            {

                StringBuilder cacheKey = new StringBuilder();
                cacheKey.Append($"careers_page_{queryParameters.Page}_pageSize_{queryParameters.RowsPerPage}_sortBy_{queryParameters.Sorting}");

                if (queryParameters.CategoryIDs != null)
                {
                    var sortedCategoryIDs = queryParameters.CategoryIDs.OrderBy(c => c);
                    cacheKey.Append($"_categories_{string.Join(",", sortedCategoryIDs)}");
                }

                if (queryParameters.AverageRatingRange != null)
                {
                    cacheKey.Append($"_average_rating_from_{queryParameters.AverageRatingRange.RatingFrom}_to_{queryParameters.AverageRatingRange.RatingTo}");
                }

                if (queryParameters.SalaryFilterQuery != null)
                {
                    cacheKey.Append($"_salary_experience_{queryParameters.SalaryFilterQuery.ExperienceYears}_" +
                        $"from_{queryParameters.SalaryFilterQuery.AverageSalaryMin}_" +
                        $"to_{queryParameters.SalaryFilterQuery.AverageSalaryMax}");
                }

                if (queryParameters.EducationTimeRange != null)
                {
                    cacheKey.Append($"_education_time_range_from_{queryParameters.EducationTimeRange.EducationTimeMin}_" +
                        $"to_{queryParameters.EducationTimeRange.EducationTimeMax}");
                }

                if (queryParameters.FilterParameters != null)
                {
                    var sortedFilterParameters = queryParameters.FilterParameters.OrderBy(fp => fp.CareerCharacteristicId);

                    foreach (var parameter in sortedFilterParameters)
                    {
                        cacheKey.Append($"_parameterID_{parameter.CareerCharacteristicId}_" +
                        $"from_{parameter.ValueFrom}_" +
                            $"to_{parameter.ValueTo}");
                    }
                }

                if (!_cache.TryGetValue(cacheKey, out GetListCareerServiceResponseModel cachedCareersListResponseModel))
                {
                    _logger.LogInformation("Cache miss. Fetching data from DB.");

                    var careerListToBeReturned = _dbContext.Careers
                    .Select(c => new CareerDto
                    {
                        Career = c,
                        AverageRating = _dbContext.Reviews
                        .Where(review => review.IsApproved && review.CareerId == c.Id)
                        .Select(review => review.Rating)
                        .DefaultIfEmpty(0)
                        .Average(),
                        AverageSalary = _dbContext.SalaryReports
                        .Where(salary => salary.IsApproved && salary.CareerId == c.Id)
                        .Select(salary => salary.Salary)
                        .DefaultIfEmpty(0)
                        .Average(),
                        TotalReviews = _dbContext.Reviews
                        .Where(review => review.IsApproved && review.CareerId == c.Id)
                        .Count(),
                        MaxSalary = _dbContext.SalaryReports
                        .Where(salary => salary.IsApproved && salary.CareerId == c.Id)
                        .Select(salary => salary.Salary)
                        .DefaultIfEmpty(0)
                        .Max(),
                        MinSalary = _dbContext.SalaryReports
                        .Where(salary => salary.IsApproved && salary.CareerId == c.Id)
                        .Select(salary => salary.Salary)
                        .DefaultIfEmpty(0)
                        .Min(),
                        CategoryName = _dbContext.Categories
                        .Where(category => category.Id == c.CategoryId)
                        .Select(category => category.Name)
                        .FirstOrDefault(),
                        AverageEducationTime = _dbContext.EducationOptions
                        .Where(edOpt => edOpt.CareerId == c.Id)
                        .Select(edOpt => edOpt.TimeLength)
                        .DefaultIfEmpty(0)
                        .Average(),
                        CareerImage = _dbContext.Images
                        .Where(image => image.Id == c.Id)
                        .Select(image => image.Base64ImageData)
                        .FirstOrDefault()
                    });

                    if (queryParameters.FilterParameters != null || queryParameters.CategoryIDs != null 
                        || queryParameters.AverageRatingRange != null || queryParameters.SalaryFilterQuery != null
                        || queryParameters.EducationTimeRange != null)
                    {

                        _logger.LogDebug("Query Parameters checked: {@QueryParameters}", queryParameters);


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
                                    careerListToBeReturned = careerListToBeReturned.Where(c => _dbContext.CharacteristicReviews
                                                      .Where(ccr => ccr.CareerCharacteristicId == parameter.CareerCharacteristicId && ccr.IsApproved)
                                                      .GroupBy(ccr => ccr.CareerId)
                                                      .Select(group => group.Any() ? group.Average(ccr => ccr.Rating) : (decimal?)0m)
                                                      .FirstOrDefault() >= parameter.ValueFrom &&
                                                    _dbContext.CharacteristicReviews
                                                      .Where(ccr => ccr.CareerCharacteristicId == parameter.CareerCharacteristicId && ccr.IsApproved)
                                                      .GroupBy(ccr => ccr.CareerId)
                                                      .Select(group => group.Any() ? group.Average(ccr => ccr.Rating) : (decimal?)0m)
                                                      .FirstOrDefault() <= parameter.ValueTo);

                                    if (!careerListToBeReturned.Any())
                                    {
                                        return new GetListCareerServiceResponseModel
                                        {
                                            Success = true,
                                            Careers = null,
                                            TotalPages = 0,
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

                                        var filteredCareerIds = mostCommonRatingStringsQuery
                                            .Where(result => parameter.StringValues.Any(str => str == result.MostCommonRatingString))
                                            .Select(result => result.CareerId);

                                        careerListToBeReturned = careerListToBeReturned.Where(c => filteredCareerIds.Contains(c.Career.Id));
                                    }

                                    if (!careerListToBeReturned.Any())
                                    {
                                        return new GetListCareerServiceResponseModel
                                        {
                                            Success = true,
                                            Careers = null,
                                            TotalPages = 0,
                                            ServerMessage = $"There are no careers matching filtering criteria."
                                        };
                                    }
                                }
                            }
                        }

                        if (queryParameters.CategoryIDs != null && queryParameters.CategoryIDs.Length > 0)
                        {
                            careerListToBeReturned = careerListToBeReturned
                                .Where(career => queryParameters.CategoryIDs.Contains(career.Career.CategoryId));

                            if (!careerListToBeReturned.Any())
                            {
                                return new GetListCareerServiceResponseModel
                                {
                                    Success = false,
                                    Careers = null,
                                    TotalPages = 0,
                                    ServerMessage = $"There are no careers matching filtering criteria."
                                };
                            }
                        }

                        if (queryParameters.AverageRatingRange != null)
                        {
                            careerListToBeReturned = careerListToBeReturned
                                .Where(result => result.AverageRating >= queryParameters.AverageRatingRange.RatingFrom &&
                                                 result.AverageRating <= queryParameters.AverageRatingRange.RatingTo);

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
                                .Where(result =>
                                    result.AverageSalary >= queryParameters.SalaryFilterQuery.AverageSalaryMin &&
                                    result.AverageSalary <= queryParameters.SalaryFilterQuery.AverageSalaryMax);

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
                            careerListToBeReturned = careerListToBeReturned
                                .Where(result =>
                                    result.AverageEducationTime >= queryParameters.EducationTimeRange.EducationTimeMin &&
                                    result.AverageEducationTime <= queryParameters.EducationTimeRange.EducationTimeMax);

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

                        var careerListResponseModel = new GetListCareerServiceResponseModel
                        {
                            Careers = mappedResult.MappedCareers,
                            ServerMessage = $"List of careers successfully collected.",
                            Success = true,
                            TotalPages = paginationResult.TotalPages
                        };

                        var cacheOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                        };


                        _careerCacheKeys.Add(cacheKey.ToString());
                        _cache.Set(cacheKey, careerListResponseModel, cacheOptions);

                        return careerListResponseModel;
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
                        if (mappedResult.Success != true)
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
                else
                {
                    _logger.LogInformation("Cache hit. Returning cached result.");

                    return cachedCareersListResponseModel;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while fetching careers.");

                return new GetListCareerServiceResponseModel
                {
                    Careers = null,
                    ServerMessage = $"List of careers collection failed: {ex.Message}.",
                    Success = false,
                    TotalPages = 0
                };
            }
        }

        private async Task<ListModel> SortList(IQueryable<CareerDto> careerListToBeSorted, CareerListSortingOption sorting)
        {
            try
            {
                switch (sorting)
                {
                    case CareerListSortingOption.ByRecommended:
                        careerListToBeSorted = careerListToBeSorted.OrderBy(c => _hashHelper.ComputeHash(c.Career.Name));
                        break;

                    case CareerListSortingOption.ByRatingAsc:
                        careerListToBeSorted = careerListToBeSorted.OrderBy(c => c.AverageRating);
                        break;

                    case CareerListSortingOption.ByRatingDesc:
                        careerListToBeSorted = careerListToBeSorted.OrderByDescending(c => c.AverageRating);

                        break;

                    case CareerListSortingOption.ByAverageWagesAsc:
                        careerListToBeSorted = careerListToBeSorted.OrderBy(c => c.AverageSalary);

                        break;

                    case CareerListSortingOption.ByAverageWagesDesc:
                        careerListToBeSorted = careerListToBeSorted.OrderByDescending(c => c.AverageSalary);
                        break;

                    default:
                        return new ListModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = "Unknown error during sorting."
                        };
                }

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

        private async Task<ListModel> PaginateList(IQueryable<CareerDto> careerListToBePaged, int page, int rowsPerPage)
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

        private async Task<ListModel> MapCareersIntoCareerListModel(IQueryable<CareerDto> careerListToBeMapped)
        {
            try
            {
                var mappedCareers = new List<CareerListModel>();

                foreach (var career in careerListToBeMapped)
                {
                    var careerListModel = new CareerListModel
                    {
                        Id = career.Career.Id,
                        Name = career.Career.Name,
                        CategoryName = career.CategoryName,
                        SalaryRange = new SalaryRange {
                            SalaryMax = career.MaxSalary,
                            SalaryMin = career.MinSalary
                        },
                        AverageReviewAndReviewCount = new AverageReviewRatingAndReviewCount
                        {
                            AverageReviewRating = career.AverageRating,
                            ReviewCount = career.TotalReviews
                        }
                    };

                    var imageResponse = await _imageService.GetImage(career.Career.ImageId);

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
                var salaryReports = await _dbContext.SalaryReports
                    .Where(sr => sr.CareerId == careerId && sr.IsApproved)
                    .Select(sr => sr.Salary)
                    .ToListAsync();

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

        private async Task<AverageReviewRatingResponse> GetAverageReviewAndReviewCount(int careerId)
        {
            try
            {
                var reviews = await _dbContext.Reviews
                .Where(review => review.IsApproved && review.CareerId == careerId)
                .ToListAsync();

                decimal averageRating = reviews.Any() ? reviews.Average(review => review.Rating) : 0m;
                int reviewCount = reviews.Count;

                return new AverageReviewRatingResponse
                {
                    Success = true,
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

        private void ClearCache()
        {
            foreach (string careerCacheKey in _careerCacheKeys)
            {
                _cache.Remove(careerCacheKey);

            }
        }
    }
}