using Lesson_17_Entity_Framework.Entities.DbContexts;
using Lesson_17_Entity_Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Lesson_17_Entity_Framework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionsController : ControllerBase
    {

        private readonly CareerContext _db;

        public ProfessionsController(CareerContext professionsContext)
        {
            this._db = professionsContext;
        }

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

        [HttpPost]
        [Route("AddProfession")]
        public async Task<ProfessionModel> AddProfession(ProfessionAddDto profession)
        {

            var model = new ProfessionAddDto
            {
                Name = profession.Name,
                Description = profession.Description,

            };

            var entity = new Entities.UserEntity
            {

                Name = model.Name,
                Description = model.Description

            };

            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();


            return new ProfessionModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
            };
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
    }
}
