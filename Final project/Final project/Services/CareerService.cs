using Final_project.Entities;
using Final_project.Entities.DbContexts;
using Final_project.Models.General;
using Final_project.Models.GET_models;
using Final_project.Models.POST;
using Final_project.Models.QueryModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;

namespace Final_project.Services
{
    public class CareerService : ICareerService
    {
        public List<CareerEntity> Careers { get; } = new List<CareerEntity>();

        private readonly CareerContext _dbContext;

        public CareerService(CareerContext dbContext)
        {
            _dbContext = dbContext;
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

                // Validate file type
                if (!IsFileOfTypeValid(addCareerModel.CareerImage))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Invalid file type. Only JPEG or PNG files are allowed."
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
                if (!CategoryExists(addCareerModel.CategoryId))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Provided career categoryId doesn't exist."
                    };
                }
                if (IsNameUnique(addCareerModel.Name))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Careeer with such name already exists."
                    };
                }

                using (var stream = new MemoryStream())
                {
                    addCareerModel.CareerImage.CopyTo(stream);
                    string base64ImageData = Convert.ToBase64String(stream.ToArray());

                    // Process the base64ImageData and save it to the database if needed
                    // Example: Save to database
                    var careerDto = new CareerDto
                    {
                        Name = addCareerModel.Name,
                        Description = addCareerModel.Description,
                        CategoryId = addCareerModel.CategoryId,
                        CareerImage = base64ImageData,
                    };

                    var careerEntity = new CareerEntity
                    {
                        Name = careerDto.Name,
                        Description = careerDto.Description,
                        CategoryId = careerDto.CategoryId,
                        Base64ImageData = careerDto.CareerImage,
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


                if (editCareerModel.CareerImage == null || editCareerModel.CareerImage.Length == 0)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"You need to upload a career image."
                    };
                }

                // Validate file type
                if (!IsFileOfTypeValid(editCareerModel.CareerImage))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Invalid file type. Only JPEG or PNG files are allowed."
                    };
                }

                if (string.IsNullOrWhiteSpace(editCareerModel.Name))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career name can't be empty."
                    };
                }
                if (string.IsNullOrWhiteSpace(editCareerModel.Description))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career description can't be empty."
                    };
                }
                if (existingCareer.IsDeleted)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"This career is no longer available and can't be modified."
                    };
                }
                if (editCareerModel.CategoryId == 0)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career categoryId can't be empty."
                    };
                }
                if (!CategoryExists(editCareerModel.CategoryId))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Provided career categoryId doesn't exist."
                    };
                }
                if (IsNameUnique(editCareerModel.Name))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Careeer with such name already exists."
                    };
                }

                using (var stream = new MemoryStream())
                {
                    editCareerModel.CareerImage.CopyTo(stream);
                    string base64ImageData = Convert.ToBase64String(stream.ToArray());

                    // Process the base64ImageData and save it to the database if needed
                    // Example: Save to database
                    var careerDto = new CareerDto
                    {
                        Name = editCareerModel.Name,
                        Description = editCareerModel.Description,
                        CategoryId = editCareerModel.CategoryId,
                        CareerImage = base64ImageData,
                    };

                    var careerEntity = new CareerEntity
                    {
                        Name = careerDto.Name,
                        Description = careerDto.Description,
                        CategoryId = careerDto.CategoryId,
                        Base64ImageData = careerDto.CareerImage,
                    };

                    await _dbContext.SaveChangesAsync();


                    // Process other fields in careerAddModel

                    return new CareerServiceResponseModel
                    {
                        Success = true,
                        Career = careerEntity,
                        ServerMessage = $"Career has been edited successfully."
                    };
                }
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

                IFormFile formFile = null;

                // Assume Base64ImageData is the Base64 string from your entity
                string base64ImageData = existingCareer.Base64ImageData;

                // Decode the Base64 string into a byte array
                byte[] fileBytes = Convert.FromBase64String(base64ImageData);

                // Create a MemoryStream from the byte array
                using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                {
                    // Create an IFormFile
                    formFile = new FormFile(memoryStream, 0, memoryStream.Length, "CareerImage", "CareerImage.jpg");

                    // Now you can use the 'formFile' as needed, for example, save it to disk, process it, etc.
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
                    CareerImage = formFile,
                    categoryName = careerCategory.Name,
                    SalaryRange = new SalaryRange { SalaryMin = minSalary,  SalaryMax = maxSalary },
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

                if (requestedCareer == null)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"Career with id {careerId} not found."
                    };
                }
                if (requestedCareer.IsDeleted)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        Career = null,
                        ServerMessage = $"This career is no longer available."
                    };
                }

                requestedCareer.IsDeleted = true;

                await _dbContext.SaveChangesAsync();

                return new CareerServiceResponseModel
                {
                    Success = true,
                    Career = new CareerEntity 
                    {
                    Id = requestedCareer.Id,
                    Name = requestedCareer.Name,
                    Description = requestedCareer.Description,
                    CategoryId = requestedCareer.CategoryId,
                    Base64ImageData = requestedCareer.Base64ImageData,
                    IsDeleted = requestedCareer.IsDeleted
                    },
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
    [Required] int page,
    [Required] int rowsPerPage,
    [Required] string sorting,
    GetCareersListCharacteristicFilterParameters[]? filterParameters,
    string[]? categoryNames,
    AverageRatingRange? averageRatingRange,
    SalaryFilterQuery? salaryFilterQuery,
    EducationTimeRange? educationTimeRange
    )
        {
            try
            {
                if (string.IsNullOrEmpty(sorting))
                {
                    return new GetListCareerServiceResponseModel
                    {
                        Success = false,
                        Careers = null,
                        ServerMessage = $"Sorting parameter can't be empty."
                    };
                }

                if (page <= 0)
                {
                    return new GetListCareerServiceResponseModel
                    {
                        Success = false,
                        Careers = null,
                        ServerMessage = $"Page parameter can't be empty."
                    };
                }

                if (rowsPerPage <= 0)
                {
                    return new GetListCareerServiceResponseModel
                    {
                        Success = false,
                        Careers = null,
                        ServerMessage = $"RowsPerPage parameter can't be empty."
                    };
                }

                int[] allowedRowsPerPage = { 5, 10, 15, 25, 50 };


                if (!allowedRowsPerPage.Contains(rowsPerPage))
                {
                    return new GetListCareerServiceResponseModel
                    {
                        Success = false,
                        Careers = null,
                        ServerMessage = $"You can only pick 5, 10, 15, 25, 50 items per page."
                    };
                }

                var filteredCareers = new List<CareerListModel>();

                if (filterParameters != null)
                {
                    foreach (var filter in filterParameters)
                    {
                        var characteristic = await _dbContext.Characteristics.FindAsync(filter.CareerCharacteristicId);

                        if (characteristic == null)
                        {
                            return new GetListCareerServiceResponseModel
                            {
                                Success = false,
                                Careers = null,
                                ServerMessage = $"Career characteristic with Id {filter.CareerCharacteristicId} not found."
                            };
                        }

                        if (characteristic.Type == "int")
                        {
                            var averageRatingsQuery = await _dbContext.CharacteristicReviews
                                .Where(ccr => ccr.CareerCharacteristicId == filter.CareerCharacteristicId && ccr.IsApproved)
                                .GroupBy(ccr => ccr.CareerId)
                                .Select(group => new
                                {
                                    CareerId = group.Key,
                                    AverageRating = group.Average(ccr => ccr.Rating) ?? 0m
                                })
                                .Where(result => result.AverageRating >= filter.ValueFrom && result.AverageRating <= filter.ValueTo)
                                .ToListAsync();

                            var mappedCareers = await _dbContext.Careers
                                .Where(c => averageRatingsQuery.Select(result => result.CareerId).Contains(c.Id))
                                .Select(c => new CareerListModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    ParameterValues = new ParameterValues[]
                                    {
                                        new ParameterValues
                                        {
                                        Id = averageRatingsQuery.FirstOrDefault(r => r.CareerId == c.Id).CareerId,
                                        Name = _dbContext.Characteristics
                                        .Where(ce => ce.Id == filter.CareerCharacteristicId)
                                        .Select(ce => ce.Name)
                                        .FirstOrDefault(),
                                        DecimalValue = averageRatingsQuery.FirstOrDefault(r => r.CareerId == c.Id).AverageRating
                                        }

                                    }
                                })
                                .ToListAsync();

                            filteredCareers.AddRange(mappedCareers);

                            if (!filteredCareers.Any())
                            {
                                return new GetListCareerServiceResponseModel
                                {
                                    Success = false,
                                    Careers = null,
                                    ServerMessage = $"There are no careers matching filtering criteria."
                                };
                            }
                        }

                        if (characteristic.Type == "string")
                        {
                            var mostCommonRatingStringsQuery = await _dbContext.CharacteristicReviews
                                .Where(ccr => ccr.CareerCharacteristicId == filter.CareerCharacteristicId && ccr.IsApproved)
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
                                })
                                .ToListAsync();

                            var filteredCareerIds = mostCommonRatingStringsQuery
                                .Where(result => filter.StringValues.Contains(result.MostCommonRatingString))
                                .Select(result => result.CareerId)
                                .ToList();

                            var mappedCareers = await _dbContext.Careers
                                .Where(c => filteredCareerIds.Contains(c.Id))
                                .Select(c => new CareerListModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    ParameterValues = new ParameterValues[] // Corrected to create an array
                                    {
                                    new ParameterValues
                                    {
                                        Id = mostCommonRatingStringsQuery.FirstOrDefault(r => r.CareerId == c.Id).CareerId,
                                        Name = _dbContext.Characteristics
                                        .Where(ce => ce.Id == filter.CareerCharacteristicId)
                                        .Select(ce => ce.Name)
                                        .FirstOrDefault() ?? "Not reviewed by users",
                                        StringValue = mostCommonRatingStringsQuery.FirstOrDefault(r => r.CareerId == c.Id).MostCommonRatingString
                                    }
                                    }
                                })
                                .ToListAsync();

                            filteredCareers.AddRange(mappedCareers);

                            if (!filteredCareers.Any())
                            {
                                return new GetListCareerServiceResponseModel
                                {
                                    Success = false,
                                    Careers = null,
                                    ServerMessage = $"There are no careers matching filtering criteria."
                                };
                            }
                        }
                    }
                }

                if (categoryNames != null && categoryNames.Length > 0)
                {
                    List<CareerListModel> joinedCareers;

                    if (filteredCareers.Any())
                    {
                        // Perform join if filteredCareers is not empty
                        joinedCareers = filteredCareers
                            .Join(_dbContext.Careers,
                                career => career.Id,
                                c => c.Id,
                                (careerListModel, careerEntity) => new
                                {
                                    CareerListModel = careerListModel,
                                    CategoryId = careerEntity.CategoryId
                                })
                            .Join(_dbContext.Categories,
                                joinedCareer => joinedCareer.CategoryId,
                                category => category.Id,
                                (joinedCareer, category) => new CareerListModel
                                {
                                    Id = joinedCareer.CareerListModel.Id,
                                    Name = joinedCareer.CareerListModel.Name,
                                    CategoryName = category.Name,
                                    ParameterValues = joinedCareer.CareerListModel.ParameterValues
                                })
                            .ToList();
                    }
                    else
                    {
                        // Filter Careers directly if filteredCareers is empty
                        joinedCareers = _dbContext.Careers
                            .Join(_dbContext.Categories,
                                career => career.CategoryId,
                                category => category.Id,
                                (career, category) => new CareerListModel
                                {
                                    Id = career.Id,
                                    Name = career.Name,
                                    CategoryName = category.Name
                                })
                            .ToList();
                    }

                    // Filter out careers based on categoryNames
                    var filteredAndMappedCareers = joinedCareers
                        .Where(career => categoryNames.Contains(career.CategoryName))
                        .ToList();

                    filteredCareers = filteredAndMappedCareers;

                    if (!filteredCareers.Any())
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"There are no careers matching filtering criteria."
                        };
                    }
                }

                if (averageRatingRange != null)
                {
                    List<CareerListModel> filteredCareersWithAverageRating;

                    if (filteredCareers.Any())
                    {
                        // Join ReviewEntity to fetch all reviews for each career
                        var joinedCareersWithReviews = filteredCareers
                            .GroupJoin(_dbContext.Reviews.Where(review => review.IsApproved),
                                career => career.Id,
                                review => review.CareerId,
                                (career, reviews) => new
                                {
                                    Career = career,
                                    Reviews = reviews
                                })
                            .ToList();

                        // Calculate average rating and review count for each career
                        var careersWithAverageRating = joinedCareersWithReviews
                            .Select(joinedCareer =>
                            {
                                var averageRating = joinedCareer.Reviews.Any()
                                    ? joinedCareer.Reviews.Average(review => review.Rating)
                                    : 0m;

                                var reviewCount = joinedCareer.Reviews.Count();

                                return new
                                {
                                    Career = joinedCareer.Career,
                                    AverageRating = averageRating,
                                    ReviewCount = reviewCount
                                };
                            })
                            .ToList();

                        // Filter careers based on averageRatingRange
                        filteredCareersWithAverageRating = careersWithAverageRating
                            .Where(result => result.AverageRating >= averageRatingRange.RatingFrom &&
                                             result.AverageRating <= averageRatingRange.RatingTo)
                            .Select(result => new CareerListModel
                            {
                                Id = result.Career.Id,
                                Name = result.Career.Name,
                                SalaryRange = result.Career.SalaryRange,
                                CategoryName = result.Career.CategoryName, // Assuming CategoryName is already populated
                                CareerImage = result.Career.CareerImage,
                                AverageReviewAndReviewCount = new AverageReviewRatingAndReviewCount
                                {
                                    AverageReviewRating = result.AverageRating,
                                    ReviewCount = result.ReviewCount
                                },
                                ParameterValues = result.Career.ParameterValues
                            })
                            .ToList();
                    }
                    else
                    {
                        // If filteredCareers is empty, filter the entire Careers table
                        var joinedCareersWithReviews = _dbContext.Careers
                            .GroupJoin(_dbContext.Reviews.Where(review => review.IsApproved),
                                career => career.Id,
                                review => review.CareerId,
                                (career, reviews) => new
                                {
                                    Career = career,
                                    Reviews = reviews
                                }).
                                ToList();


                        // Calculate average rating and review count for each career
                        var careersWithAverageRating = joinedCareersWithReviews
                            .Select(joinedCareer =>
                            {
                                var averageRating = joinedCareer.Reviews.Any()
                                    ? joinedCareer.Reviews.Average(review => review.Rating)
                                    : 0m;

                                var reviewCount = joinedCareer.Reviews.Count();

                                return new
                                {
                                    Career = joinedCareer.Career,
                                    AverageRating = averageRating,
                                    ReviewCount = reviewCount
                                };
                            })
                            .ToList();


                        filteredCareersWithAverageRating = careersWithAverageRating
                            .Where(result => result.AverageRating >= averageRatingRange.RatingFrom &&
                                             result.AverageRating <= averageRatingRange.RatingTo)
                            .Select(result => new CareerListModel
                            {
                                Id = result.Career.Id,
                                Name = result.Career.Name,
                                AverageReviewAndReviewCount = new AverageReviewRatingAndReviewCount
                                {
                                    AverageReviewRating = result.AverageRating,
                                    ReviewCount = result.ReviewCount
                                },
                            })
                            .ToList();
                    }

                    // Update filteredCareers with the filtered list
                    filteredCareers = filteredCareersWithAverageRating;

                    if (!filteredCareers.Any())
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"There are no careers matching filtering criteria."
                        };
                    }
                }

                if (salaryFilterQuery != null)
                {
                    List<CareerListModel> filteredCareersWithAverageSalary;

                    if (filteredCareers.Any())
                    {
                        // Join SalaryReportEntity to fetch all salary reports for each career
                        var joinedCareersWithSalaryReports = filteredCareers
                            .GroupJoin(_dbContext.SalaryReports.Where(salary => salary.IsApproved),
                                career => career.Id,
                                salaryReport => salaryReport.CareerId,
                                (career, salaryReports) => new
                                {
                                    Career = career,
                                    SalaryReports = salaryReports
                                })
                            .ToList();

                        // Calculate average salary for each career based on ExperienceYears
                        var careersWithAverageSalary = joinedCareersWithSalaryReports
                            .Select(joinedCareer =>
                            {
                                var matchingSalaryReports = joinedCareer.SalaryReports
                                    .Where(sr => sr.ExperienceYears == salaryFilterQuery.ExperienceYears)
                                    .ToList();

                                var averageSalary = matchingSalaryReports.Any()
                                    ? matchingSalaryReports.Average(sr => sr.Salary)
                                    : 0;

                                var minSalary = matchingSalaryReports.Any()
                                    ? matchingSalaryReports.Min(sr => sr.Salary)
                                    : 0;

                                var maxSalary = matchingSalaryReports.Any()
                                    ? matchingSalaryReports.Max(sr => sr.Salary)
                                    : 0;

                                return new
                                {
                                    Career = joinedCareer.Career,
                                    AverageSalary = averageSalary,
                                    MinSalary = minSalary,
                                    MaxSalary = maxSalary
                                };
                            })
                            .ToList();

                        // Filter careers based on SalaryFilterQuery
                        filteredCareersWithAverageSalary = careersWithAverageSalary
                            .Where(result =>
                                result.AverageSalary >= salaryFilterQuery.AverageSalaryMin &&
                                result.AverageSalary <= salaryFilterQuery.AverageSalaryMax)
                            .Select(result =>
                            {
                                var updatedCareer = new CareerListModel
                                {
                                    Id = result.Career.Id,
                                    Name = result.Career.Name,
                                    CategoryName = result.Career.CategoryName, // Assuming CategoryName is already populated
                                    AverageReviewAndReviewCount = result.Career.AverageReviewAndReviewCount,
                                    ParameterValues = result.Career.ParameterValues,
                                    SalaryRange = new SalaryRange
                                    {
                                        SalaryMax = result.MaxSalary,
                                        SalaryMin = result.MinSalary
                                    }
                                };

                                // Add the new ParameterValues to existing ones
                                updatedCareer.ParameterValues = result.Career.ParameterValues ?? new ParameterValues[0];
                                updatedCareer.ParameterValues = updatedCareer.ParameterValues
                                    .Concat(new[]
                                    {
                                        new ParameterValues
                                        {
                                            Name = $"Average salary ({salaryFilterQuery.ExperienceYears} experience",
                                            DecimalValue = (decimal?)result.AverageSalary
                                        }
                                    })
                                    .ToArray();

                                return updatedCareer;
                            })
                            .ToList();
                    }
                    else
                    {
                        // If filteredCareers is empty, filter the entire Careers table
                        var joinedCareersWithSalaryReports = _dbContext.Careers
                            .GroupJoin(_dbContext.SalaryReports.Where(salary => salary.IsApproved),
                                career => career.Id,
                                salaryReport => salaryReport.CareerId,
                                (career, salaryReports) => new
                                {
                                    Career = career,
                                    SalaryReports = salaryReports
                                })
                            .ToList();

                        // Calculate average salary for each career based on ExperienceYears
                        var careersWithAverageSalary = joinedCareersWithSalaryReports
                            .Select(joinedCareer =>
                            {
                                var matchingSalaryReports = joinedCareer.SalaryReports
                                    .Where(sr => sr.ExperienceYears == salaryFilterQuery.ExperienceYears)
                                    .ToList();

                                var averageSalary = matchingSalaryReports.Any()
                                    ? matchingSalaryReports.Average(sr => sr.Salary)
                                    : 0;

                                var minSalary = matchingSalaryReports.Any()
                                    ? matchingSalaryReports.Min(sr => sr.Salary)
                                    : 0;

                                var maxSalary = matchingSalaryReports.Any()
                                    ? matchingSalaryReports.Max(sr => sr.Salary)
                                    : 0;

                                return new
                                {
                                    Career = joinedCareer.Career,
                                    AverageSalary = averageSalary,
                                    MinSalary = minSalary,
                                    MaxSalary = maxSalary
                                };
                            })
                            .ToList();

                        // Filter careers based on SalaryFilterQuery
                        filteredCareersWithAverageSalary = careersWithAverageSalary
                            .Where(result =>
                                result.AverageSalary >= salaryFilterQuery.AverageSalaryMin &&
                                result.AverageSalary <= salaryFilterQuery.AverageSalaryMax)
                            .Select(result => new CareerListModel
                            {
                                Id = result.Career.Id,
                                Name = result.Career.Name,
                                SalaryRange = new SalaryRange
                                {
                                    SalaryMax = result.MaxSalary,
                                    SalaryMin = result.MinSalary
                                }
                            })
                            .ToList();
                    }

                    // Update filteredCareers with the filtered list
                    filteredCareers = filteredCareersWithAverageSalary;

                    if (!filteredCareers.Any())
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"There are no careers matching filtering criteria."
                        };
                    }
                }

                if (educationTimeRange != null)
                {
                    List<CareerListModel> filteredCareersWithAverageEducationTime;

                    if (filteredCareers.Any())
                    {
                        // Join EducationOptionEntity to fetch all education options for each career
                        var joinedCareersWithEducationOptions = filteredCareers
                            .GroupJoin(_dbContext.EducationOptions,
                                career => career.Id,
                                educationOption => educationOption.CareerId,
                                (career, educationOptions) => new
                                {
                                    Career = career,
                                    EducationOptions = educationOptions
                                })
                            .ToList();

                        // Calculate average education time for each career
                        var careersWithAverageEducationTime = joinedCareersWithEducationOptions
                            .Select(joinedCareer =>
                            {
                                var averageEducationTime = joinedCareer.EducationOptions.Any()
                                    ? joinedCareer.EducationOptions.Average(eo => eo.TimeLength)
                                    : 0;

                                return new
                                {
                                    Career = joinedCareer.Career,
                                    AverageEducationTime = averageEducationTime
                                };
                            })
                            .ToList();

                        // Filter careers based on EducationTimeRange
                        filteredCareersWithAverageEducationTime = careersWithAverageEducationTime
                            .Where(result =>
                                result.AverageEducationTime >= educationTimeRange.EducationTimeMin &&
                                result.AverageEducationTime <= educationTimeRange.EducationTimeMax)
                            .Select(result =>
                            {
                                // Create a new instance of CareerListModel with additional ParameterValues
                                var updatedCareer = new CareerListModel
                                {
                                    Id = result.Career.Id,
                                    Name = result.Career.Name,
                                    SalaryRange = result.Career.SalaryRange,
                                    CategoryName = result.Career.CategoryName, // Assuming CategoryName is already populated
                                    AverageReviewAndReviewCount = result.Career.AverageReviewAndReviewCount
                                };

                                // Add the new ParameterValues to existing ones
                                updatedCareer.ParameterValues = result.Career.ParameterValues ?? new ParameterValues[0];
                                updatedCareer.ParameterValues = updatedCareer.ParameterValues
                                    .Concat(new[]
                                    {
                                        new ParameterValues
                                        {
                                            Name = "Education time",
                                            DecimalValue = (decimal?)result.AverageEducationTime
                                        }
                                    })
                                    .ToArray();

                                return updatedCareer;
                            })
                            .ToList();
                    }
                    else
                    {

                        // Join EducationOptionEntity to fetch all education options for each career
                        var joinedCareersWithEducationOptions = _dbContext.Careers
                            .GroupJoin(_dbContext.EducationOptions,
                                career => career.Id,
                                educationOption => educationOption.CareerId,
                                (career, educationOptions) => new
                                {
                                    Career = career,
                                    EducationOptions = educationOptions
                                })
                            .ToList();

                        // Calculate average education time for each career
                        var careersWithAverageEducationTime = joinedCareersWithEducationOptions
                            .Select(joinedCareer =>
                            {
                                var averageEducationTime = joinedCareer.EducationOptions.Any()
                                    ? joinedCareer.EducationOptions.Average(eo => eo.TimeLength)
                                    : 0;

                                return new
                                {
                                    Career = joinedCareer.Career,
                                    AverageEducationTime = averageEducationTime
                                };
                            })
                            .ToList();

                        // Filter careers based on EducationTimeRange
                        filteredCareersWithAverageEducationTime = careersWithAverageEducationTime
                            .Where(result =>
                                result.AverageEducationTime >= educationTimeRange.EducationTimeMin &&
                                result.AverageEducationTime <= educationTimeRange.EducationTimeMax)
                            .Select(result =>
                            {
                                // Create a new instance of CareerListModel with additional ParameterValues
                                var updatedCareer = new CareerListModel
                                {
                                    Id = result.Career.Id,
                                    Name = result.Career.Name,
                                };

                                // Add the new ParameterValues to existing ones
                                updatedCareer.ParameterValues = new ParameterValues[0];
                                updatedCareer.ParameterValues = updatedCareer.ParameterValues
                                    .Concat(new[]
                                    {
                                        new ParameterValues
                                        {
                                            Name = "Education time",
                                            DecimalValue = (decimal?)result.AverageEducationTime
                                        }
                                    })
                                    .ToArray();

                                return updatedCareer;
                            })
                            .ToList();
                    }

                    filteredCareers = filteredCareersWithAverageEducationTime;

                    if (!filteredCareers.Any())
                    {
                        return new GetListCareerServiceResponseModel
                        {
                            Success = false,
                            Careers = null,
                            ServerMessage = $"There are no careers matching filtering criteria."
                        };
                    }
                }

                if (filteredCareers.Any())
                {
                    foreach (var career in filteredCareers)
                    {

                        if (career.SalaryRange == null)
                        {
                            // Join SalaryReportEntity to fetch all salary reports for each career
                            var joinedCareerWithSalaryReports = _dbContext.Careers
                                .Where(c => c.Id == career.Id) // Filter by the specific career
                                .GroupJoin(_dbContext.SalaryReports.Where(salary => salary.IsApproved),
                                    careerEntity => careerEntity.Id,
                                    salaryReport => salaryReport.CareerId,
                                    (careerEntity, salaryReports) => new
                                    {
                                        CareerEntity = careerEntity,
                                        SalaryReports = salaryReports
                                    })
                                .ToList();

                            // Calculate min and max salary for the specific career based on ExperienceYears
                            var careerWithMinMaxSalary = joinedCareerWithSalaryReports
                                .Select(joinedCareer =>
                                {
                                    var matchingSalaryReports = joinedCareer.SalaryReports
                                        .Where(sr => sr.ExperienceYears == salaryFilterQuery.ExperienceYears)
                                        .ToList();

                                    var minSalary = matchingSalaryReports.Any()
                                        ? matchingSalaryReports.Min(sr => sr.Salary)
                                        : 0;

                                    var maxSalary = matchingSalaryReports.Any()
                                        ? matchingSalaryReports.Max(sr => sr.Salary)
                                        : 0;

                                    return new
                                    {
                                        Career = joinedCareer.CareerEntity,
                                        MinSalary = minSalary,
                                        MaxSalary = maxSalary
                                    };
                                })
                                .FirstOrDefault(); // Use FirstOrDefault to get the single career

                            // Update the SalaryRange for the specific career in filteredCareers

                            career.SalaryRange = new SalaryRange
                            {
                                SalaryMin = careerWithMinMaxSalary.MinSalary,
                                SalaryMax = careerWithMinMaxSalary.MaxSalary
                            };
                        }

                        if (string.IsNullOrEmpty(career.CategoryName))
                        {
                            // Join SalaryReportEntity to fetch all salary reports for each career
                            var joinedCareerWithCategory = _dbContext.Careers
                                .Where(c => c.Id == career.Id) // Filter by the specific career
                                .Join(_dbContext.Categories,
                                    careerEntity => careerEntity.CategoryId,
                                    category => category.Id,
                                    (careerEntity, category) => new
                                    {
                                        CareerEntity = careerEntity,
                                        Category = category
                                    })
                                .FirstOrDefault();

                            career.CategoryName = joinedCareerWithCategory.Category.Name;
                        }

                        if (career.CareerImage == null)
                        {
                            // Join filteredCareers with CareerEntity to fetch CareerImage
                            var careerWithImage = _dbContext.Careers
                                .Where(c => c.Id == career.Id) // Filter by the specific career
                                .Join(_dbContext.Careers,
                                    careerEntity => careerEntity.Id,
                                    givenCareer => career.Id,
                                    (careerEntity, givenCareer) => new
                                    {
                                        CareerEntity = careerEntity,
                                        Career = givenCareer
                                    })
                                .FirstOrDefault();

                            career.CareerImage = MapBase64ImageDataToFormFile((careerWithImage.CareerEntity.Base64ImageData), careerWithImage.Career.Name);
                        }

                        if (career.AverageReviewAndReviewCount == null)
                        {
                            var careerWithAverageReviewAndReviewCount = _dbContext.Careers
                                .Where(c => c.Id == career.Id) // Filter by the specific career
                                .GroupJoin(_dbContext.Reviews.Where(review => review.IsApproved),
                                    careerEntity => careerEntity.Id,
                                    reviewEntity => reviewEntity.CareerId,
                                    (careerEntity, reviewEntity) => new
                                    {
                                        CareerEntity = careerEntity,
                                        ReviewEntity = reviewEntity
                                    })
                                .FirstOrDefault();

                            var averageRating = careerWithAverageReviewAndReviewCount.ReviewEntity.Any()
                                ? careerWithAverageReviewAndReviewCount.ReviewEntity.Average(review => review.Rating)
                                : 0m;

                            var reviewCount = careerWithAverageReviewAndReviewCount.ReviewEntity.Count();

                            career.AverageReviewAndReviewCount = new AverageReviewRatingAndReviewCount
                            {
                                AverageReviewRating = averageRating,
                                ReviewCount = reviewCount
                            };
                        }
                    }
                }





                if (filteredCareers.Any())
                {
                    switch (sorting)
                    {
                        case "By recommended":
                            // Implement random sorting logic for "By recommended"
                            Random random = new Random();
                            filteredCareers = filteredCareers.OrderBy(c => random.Next()).ToList();
                            break;

                        case "By rating asc":
                            // Implement sorting logic for "By rating asc"
                            filteredCareers = filteredCareers.OrderBy(c => c.AverageReviewAndReviewCount.AverageReviewRating).ToList();

                            break;

                        case "By rating desc":
                            // Implement sorting logic for "By rating desc"
                            filteredCareers = filteredCareers.OrderByDescending(c => c.AverageReviewAndReviewCount.AverageReviewRating).ToList();
                            break;

                        case "By average wages asc":
                            // Assuming careers is the list of CareerListModel
                            filteredCareers = filteredCareers
                                .Select(career => new
                                {
                                    Career = career,
                                    AverageSalary = _dbContext.SalaryReports
                                        ?.Where(sr => sr.CareerId == career.Id && sr.IsApproved)
                                        .Select(sr => sr.Salary)
                                        .DefaultIfEmpty(0) // DefaultIfEmpty with 0 if the sequence is empty
                                        .Average()
                                })
                                .OrderBy(result => result.AverageSalary)
                                .Select(result => result.Career)
                                .ToList();
                            break;

                        case "By average wages desc":
                            filteredCareers = filteredCareers
                                .Select(career => new
                                {
                                    Career = career,
                                    AverageSalary = _dbContext.SalaryReports
                                        ?.Where(sr => sr.CareerId == career.Id && sr.IsApproved)
                                        .Select(sr => sr.Salary)
                                        .DefaultIfEmpty(0) // DefaultIfEmpty with 0 if the sequence is empty
                                        .Average()
                                })
                                .OrderByDescending(result => result.AverageSalary)
                                .Select(result => result.Career)
                                .ToList();
                            break;

                        default:
                            // If an invalid sorting parameter is applied, return an error response
                            var errorResponse = new GetListCareerServiceResponseModel
                            {
                                Success = false,
                                ServerMessage = "Invalid sorting parameter applied.",
                                Careers = null
                            };
                            return (errorResponse);
                    }
                }

                var unfilteredCareers = new List<CareerListModel>();

                if (!filteredCareers.Any())
                {
                    switch (sorting)
                    {
                        case "By recommended":
                            // Implement random sorting logic for "By recommended"
                            Random random = new Random();
                            unfilteredCareers = await _dbContext.Careers
                                .OrderBy(c => random.Next())
                                .Select(c => new CareerListModel
                                {
                                    Id = c.Id,
                                    Name = c.Name
                                })
                                .ToListAsync();
                            break;


                        case "By rating asc":
                            {
                                // Join ReviewEntity to fetch all reviews for each career
                                var joinedUnfilteredCareersWithReviews = await _dbContext.Careers
                                    .GroupJoin(_dbContext.Reviews.Where(review => review.IsApproved),
                                        career => career.Id,
                                        review => review.CareerId,
                                        (career, reviews) => new
                                        {
                                            Career = career,
                                            Reviews = reviews
                                        })
                                    .ToListAsync();

                                unfilteredCareers = joinedUnfilteredCareersWithReviews
                                    .Select(joinedCareer =>
                                    {
                                        var averageRating = joinedCareer.Reviews.Any()
                                            ? joinedCareer.Reviews.Average(review => review.Rating)
                                            : 0m;

                                        var reviewCount = joinedCareer.Reviews.Count();

                                        return new
                                        {
                                            Career = joinedCareer.Career,
                                            AverageRating = averageRating,
                                            ReviewCount = reviewCount
                                        };
                                    })
                                    .OrderBy(result => result.AverageRating) // Sort by AverageRating in descending order
                                    .Select(result => new CareerListModel
                                    {
                                        Id = result.Career.Id,
                                        Name = result.Career.Name,
                                        AverageReviewAndReviewCount = new AverageReviewRatingAndReviewCount
                                        {
                                            AverageReviewRating = result.AverageRating,
                                            ReviewCount = result.ReviewCount
                                        }
                                    })
                                    .ToList();
                            }
                            break;

                        case "By rating desc":
                            {
                                var joinedUnfilteredCareersWithReviews = await _dbContext.Careers
                                    .GroupJoin(_dbContext.Reviews.Where(review => review.IsApproved),
                                        career => career.Id,
                                        review => review.CareerId,
                                        (career, reviews) => new
                                        {
                                            Career = career,
                                            Reviews = reviews
                                        })
                                    .ToListAsync();

                                unfilteredCareers = joinedUnfilteredCareersWithReviews
                                    .Select(joinedCareer =>
                                    {
                                        var averageRating = joinedCareer.Reviews.Any()
                                            ? joinedCareer.Reviews.Average(review => review.Rating)
                                            : 0m;

                                        var reviewCount = joinedCareer.Reviews.Count();

                                        return new
                                        {
                                            Career = joinedCareer.Career,
                                            AverageRating = averageRating,
                                            ReviewCount = reviewCount
                                        };
                                    })
                                    .OrderByDescending(result => result.AverageRating) // Sort by AverageRating in descending order
                                    .Select(result => new CareerListModel
                                    {
                                        Id = result.Career.Id,
                                        Name = result.Career.Name,
                                        AverageReviewAndReviewCount = new AverageReviewRatingAndReviewCount
                                        {
                                            AverageReviewRating = result.AverageRating,
                                            ReviewCount = result.ReviewCount
                                        }
                                    })
                                    .ToList();
                            }
                            break;

                        case "By average wages asc":
                            // Assuming careers is the list of CareerListModel
                            unfilteredCareers = _dbContext.Careers
                                .Select(career => new
                                {
                                    Career = career,
                                    AverageSalary = _dbContext.SalaryReports
                                        .Where(sr => sr.CareerId == career.Id && sr.IsApproved)
                                        .Select(sr => sr.Salary)
                                        .DefaultIfEmpty(0) // DefaultIfEmpty with 0 if the sequence is empty
                                        .Average()
                                })
                                .OrderBy(result => result.AverageSalary)
                                .Select(result => new CareerListModel
                                {
                                    Id = result.Career.Id,
                                    Name = result.Career.Name
                                })
                                .ToList();
                            break;

                        case "By average wages desc":
                            unfilteredCareers = _dbContext.Careers
                                .Select(career => new
                                {
                                    Career = career,
                                    AverageSalary = _dbContext.SalaryReports
                                        .Where(sr => sr.CareerId == career.Id && sr.IsApproved)
                                        .Select(sr => sr.Salary)
                                        .DefaultIfEmpty(0) // DefaultIfEmpty with 0 if the sequence is empty
                                        .Average()
                                })
                                .OrderByDescending(result => result.AverageSalary)
                                .Select(result => new CareerListModel
                                {
                                    Id = result.Career.Id,
                                    Name = result.Career.Name
                                })
                                .ToList();
                            break;

                        default:
                            // If an invalid sorting parameter is applied, return an error response
                            var errorResponse = new GetListCareerServiceResponseModel
                            {
                                Success = false,
                                ServerMessage = "Invalid sorting parameter applied.",
                                Careers = null
                            };
                            return (errorResponse);
                    }
                }

                if (!filteredCareers.Any())
                {
                    MapCareersIntoCareerListModel(unfilteredCareers);
                }

                    // Pagination logic
                    IEnumerable<CareerListModel> pagedCareerListModels = filteredCareers.Any()
                            ? filteredCareers
                            : unfilteredCareers;

                    int totalItemCount = pagedCareerListModels.Count();

                    int totalPages = (int)Math.Ceiling((double)totalItemCount / rowsPerPage);

                    pagedCareerListModels = pagedCareerListModels
                        .Skip((page - 1) * rowsPerPage)
                        .Take(rowsPerPage);

                if (!pagedCareerListModels.Any())
                {
                    MapCareersIntoCareerListModel(unfilteredCareers);
                }

                // Transform the pagedCareerListModels to your response model
                var responseModel = new GetListCareerServiceResponseModel
                    {
                    Success = false,
                    ServerMessage = "Invalid page number.",
                    Careers = null,
                    totalPages = totalPages
                    };

                    return (responseModel);
            }
            catch (Exception ex)
            {
                return new GetListCareerServiceResponseModel
                {
                    Success = false,
                    Careers = null,
                    ServerMessage = $"Internal server error: {ex.Message}"
                };
            }
        }


        private List<CareerListModel> MapCareersIntoCareerListModel(List<CareerListModel> careers)
        {
            var mappedCareers = new List<CareerListModel>();

            mappedCareers = careers;

            foreach (var career in mappedCareers)
            {

                if (career.SalaryRange == null)
                {
                    // Join SalaryReportEntity to fetch all salary reports for each career
                    var joinedCareerWithSalaryReports = _dbContext.Careers
                        .Where(c => c.Id == career.Id) // Filter by the specific career
                        .GroupJoin(_dbContext.SalaryReports.Where(salary => salary.IsApproved),
                            careerEntity => careerEntity.Id,
                            salaryReport => salaryReport.CareerId,
                            (careerEntity, salaryReports) => new
                            {
                                CareerEntity = careerEntity,
                                SalaryReports = salaryReports
                            })
                        .ToList();

                    // Calculate min and max salary for the specific career based on ExperienceYears
                    var careerWithMinMaxSalary = joinedCareerWithSalaryReports
                        .Select(joinedCareer =>
                        {
                            var minSalary = joinedCareerWithSalaryReports.Any()
                                ? joinedCareer.SalaryReports.Min(sr => sr.Salary)
                                : 0;

                            var maxSalary = joinedCareerWithSalaryReports.Any()
                                ? joinedCareer.SalaryReports.Max(sr => sr.Salary)
                                : 0;

                            return new
                            {
                                Career = joinedCareer.CareerEntity,
                                MinSalary = minSalary,
                                MaxSalary = maxSalary
                            };
                        })
                        .FirstOrDefault(); // Use FirstOrDefault to get the single career

                    // Update the SalaryRange for the specific career in filteredCareers

                    career.SalaryRange = new SalaryRange
                    {
                        SalaryMin = careerWithMinMaxSalary.MinSalary,
                        SalaryMax = careerWithMinMaxSalary.MaxSalary
                    };
                }

                if (string.IsNullOrEmpty(career.CategoryName))
                {
                    // Join SalaryReportEntity to fetch all salary reports for each career
                    var joinedCareerWithCategory = _dbContext.Careers
                        .Where(c => c.Id == career.Id) // Filter by the specific career
                        .Join(_dbContext.Categories,
                            careerEntity => careerEntity.CategoryId,
                            category => category.Id,
                            (careerEntity, category) => new
                            {
                                CareerEntity = careerEntity,
                                Category = category
                            })
                        .FirstOrDefault();

                    career.CategoryName = joinedCareerWithCategory.Category.Name;
                }

                if (career.CareerImage == null)
                {
                    // Join filteredCareers with CareerEntity to fetch CareerImage
                    var careerWithImage = _dbContext.Careers
                        .Where(c => c.Id == career.Id) // Filter by the specific career
                        .Join(_dbContext.Careers,
                            careerEntity => careerEntity.Id,
                            givenCareer => career.Id,
                            (careerEntity, givenCareer) => new
                            {
                                CareerEntity = careerEntity,
                                Career = givenCareer
                            })
                        .FirstOrDefault();

                    career.CareerImage = MapBase64ImageDataToFormFile((careerWithImage.CareerEntity.Base64ImageData), careerWithImage.Career.Name);
                }

                if (career.AverageReviewAndReviewCount == null)
                {
                    var careerWithAverageReviewAndReviewCount = _dbContext.Careers
                        .Where(c => c.Id == career.Id) // Filter by the specific career
                        .GroupJoin(_dbContext.Reviews.Where(review => review.IsApproved),
                            careerEntity => careerEntity.Id,
                            reviewEntity => reviewEntity.CareerId,
                            (careerEntity, reviewEntity) => new
                            {
                                CareerEntity = careerEntity,
                                ReviewEntity = reviewEntity
                            })
                        .FirstOrDefault();

                    var averageRating = careerWithAverageReviewAndReviewCount.ReviewEntity.Any()
                        ? careerWithAverageReviewAndReviewCount.ReviewEntity.Average(review => review.Rating)
                        : 0m;

                    var reviewCount = careerWithAverageReviewAndReviewCount.ReviewEntity.Count();

                    career.AverageReviewAndReviewCount = new AverageReviewRatingAndReviewCount
                    {
                        AverageReviewRating = averageRating,
                        ReviewCount = reviewCount
                    };
                }
            }

            return mappedCareers;
        }

        private IFormFile MapBase64ImageDataToFormFile(string base64ImageData, string careerName)
        {
            // Decode the Base64 string into a byte array
            byte[] fileBytes = Convert.FromBase64String(base64ImageData);

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                // Create an IFormFile
                return new FormFile(memoryStream, 0, memoryStream.Length, $"{careerName}", $"{careerName}.jpg");
            }
        }

        private bool CategoryExists(int categoryId)
        {
            return _dbContext.Categories.Any(c => c.Id == categoryId);
        }

        private bool IsFileOfTypeValid(IFormFile file)
        {
            // Add logic to check if the file type is JPEG or PNG
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(extension);
        }

        private bool IsNameUnique(string name)
        {
            // Check if a record with the given name already exists
            return _dbContext.Careers.Any(e => e.Name == name);
        }

    }
}


