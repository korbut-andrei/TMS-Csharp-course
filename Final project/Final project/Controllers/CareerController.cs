using Final_project.Entities;
using Final_project.Entities.DbContexts;
using Final_project.Models;
using Final_project.Models.Auth;
using Final_project.Models.General;
using Final_project.Models.GET_models;
using Final_project.Models.POST;
using Final_project.Models.QueryModels;
using Final_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_project.Controllers
{
    [Route("api/Careers")]
    [ApiController]
    public class CareerController : ControllerBase
    {
        private readonly CareerService _careerService;

        public CareerController(CareerContext careerContext, CareerService careerService)
        {
            _careerService = careerService;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("AddCareer")]
        public async Task<IActionResult> AddCareer([FromForm] AddCareerModel addCareerModel)
        {
            var result = await _careerService.AddCareer(addCareerModel);

            if (result.Success)
            {
                return new OkObjectResult(new
                {
                    message = "Career has been created successfully.",
                    careerEntity = result.Career
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("EditCareer")]
        public async Task<IActionResult> EditCareer([FromForm] EditCareerModel editCareerModel)
        {
            var result = await _careerService.EditCareer(editCareerModel);

            if (result.Success)
            {
                return new OkObjectResult(new
                {
                    message = result.ServerMessage,
                    careerEntity = result.Career
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

        [HttpPost]
        [Route("GetCareer")]
        public async Task<IActionResult> GetCareer([FromQuery] int careerId)
        {
            var result = await _careerService.GetCareer(careerId);

            if (result.Success)
            {
                return new OkObjectResult(new
                {
                    message = result.ServerMessage,
                    careerEntity = result.Career
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

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("GetCareer")]
        public async Task<IActionResult> DeleteCareer([FromQuery] int careerId)
        {
            var result = await _careerService.DeleteCareer(careerId);

            if (result.Success)
            {
                return new OkObjectResult(new
                {
                    message = result.ServerMessage,
                    careerEntity = result.Career
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

    public async Task<IActionResult> GetCareersByDynamicFiltersAndCategories(
    [FromQuery] List<GetCareersListCharacteristicFilterParameters>? filterParameters,
    [FromQuery] string[]? categoryNames,
    [FromQuery] AverageRatingRange? averageRatingRange,
    [FromQuery] SalaryRange? salaryRange,
    [FromQuery] SalaryFilterQuery? salaryFilterQuery,
    [FromQuery] EducationTimeRange? educationTime
    )


        /*
        [HttpGet]
        public async Task<ActionResult<ProfessionsListModel>> GetProfessions(
            [FromQuery][Required] int page,
            [FromQuery][Required] int rowsPerPage,
            [FromQuery] string sortField = null,
            [FromQuery] string sortOrder = null,
            [FromQuery] string groupBy = null,
            [FromQuery] string filterField = null,
            [FromQuery] string filterString = null)
        {
            // Initialize an error message variable
            string errorMessage = null;

            // Start with the base query
            var baseQuery = _db.Professions
                .Select(profession => new ProfessionsListEntryModel
                {
                    Id = profession.Id,
                    Name = profession.Name,
                    SalaryRangeMin = profession.SalaryRangeMim,
                    SalaryRangeMax = profession.SalaryRangeMax,
                    Description = profession.Description,
                    Reviews = profession.Reviews.Select(review => new ProfessionsListReviewEntryModel
                    {
                        Id = review.Id,
                        Rating = review.Rating
                    }).ToArray(),
                });

            var prop = typeof(Entities.UserEntity).GetProperty(filterField);

            // Apply filtering based on the request
            if (prop != null) 
            {
                if (prop.PropertyType == typeof(string) && !string.IsNullOrEmpty(filterString))
                {
                    baseQuery = baseQuery.Where(profession =>
                EF.Property<string>(profession, filterField).Contains(filterString));
                }

                if (prop.PropertyType == typeof(string) && int.TryParse(filterString, out var parsedInt))
                {
                    baseQuery = baseQuery.Where(profession =>
                EF.Property<int>(profession, filterField) == parsedInt);
                }
            }

            // Apply grouping
            var groupedQuery = baseQuery.GroupBy(profession => EF.Property<object>(profession, groupBy))
                .Select(group => new
                {
                    GroupKey = group.Key,
                    Items = group.ToList()
                });

            // Check for errors
            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Return a BadRequest with an error message
                return BadRequest(errorMessage);
            }

            // Apply sorting based on the request
            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortOrder?.ToLower())
                {
                    case "asc":
                        groupedQuery = groupedQuery.OrderBy(group => EF.Property<object>(group, sortField));
                        break;
                    case "desc":
                        groupedQuery = groupedQuery.OrderByDescending(group => EF.Property<object>(group, sortField));
                        break;
                }
            }

            // Handle paging
            IQueryable<ProfessionsListEntryModel> pagedQuery;

            if (rowsPerPage >= 1 && rowsPerPage <= 100)
            {
                pagedQuery = groupedQuery
                    .Skip((page - 1) * rowsPerPage)
                    .Take(rowsPerPage)
                    .SelectMany(group => group.Items);
            }
            else
            {
                // Retrieve all data in a single page
                pagedQuery = groupedQuery
                    .SelectMany(group => group.Items)
                    .AsQueryable();
            }

            // Materialize the query asynchronously
            var pagedProfessions = await pagedQuery.ToArrayAsync();

            // Return the result
            return new ProfessionsListModel
            {
                Professions = pagedProfessions
            };
        }

        [HttpGet]
        [Route("GetProfession")]
        public Entities.UserEntity GetProfession(int id)
        {
            return _db.Professions.Where(x => x.Id == id).FirstOrDefault();
        }

        [HttpPut]
        [Route("UpdateProfession")]
        public string UpdateProfession(Entities.UserEntity profession)
        {
            _db.Entry(profession).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();

            return "User has been successfully updated.";
        }

        [HttpDelete]
        [Route("DeleteProfession/{id}")]
        public string DeleteProfession(int id)
        {
            // Ensure that only the ID is passed for deletion
            if (ModelState.IsValid)
            {
                // Retrieve the profession by ID
                Entities.UserEntity professionToDelete = _db.Professions.Find(id);

                // Check if the profession exists
                if (professionToDelete != null)
                {
                    // Remove the profession
                    _db.Professions.Remove(professionToDelete);
                    _db.SaveChanges();

                    return "Profession has been successfully deleted.";
                }
                else
                {
                    return "Profession not found.";
                }
            }
            else
            {
                return "Invalid model state. Please provide a valid ID.";
            }
        }
        */
    }
}
