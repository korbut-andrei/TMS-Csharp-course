using Final_project.Entities.DbContexts;
using Final_project.Entities;
using Final_project.Models.POST;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Services
{
    public class CategoryService : ICategoryService
    {
        public List<CategoryEntity> Categories { get; } = new List<CategoryEntity>();

        private readonly CareerContext _dbContext;

        public CategoryService(CareerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> AddCategory(AddCategoryModel addCategoryModel)
        {
            try
            {
                if (addCategoryModel.CategoryImage == null || addCategoryModel.CategoryImage.Length == 0)
                {
                    return new BadRequestObjectResult("You need to upload a Category image");
                }

                // Validate file type
                if (!IsFileOfTypeValid(addCategoryModel.CategoryImage))
                {
                    return new BadRequestObjectResult("Invalid file type. Only JPEG or PNG files are allowed.");
                }

                if (string.IsNullOrWhiteSpace(addCategoryModel.Name))
                {
                    return new BadRequestObjectResult("Career name can't be empty.");
                }
                if (IsNameUnique(addCategoryModel.Name))
                {
                    return new BadRequestObjectResult("Category with such name already exists.");
                }

                using (var stream = new MemoryStream())
                {
                    addCategoryModel.CategoryImage.CopyTo(stream);
                    string base64ImageData = Convert.ToBase64String(stream.ToArray());

                    // Process the base64ImageData and save it to the database if needed
                    // Example: Save to database
                    var careerDto = new AddCategoryDto
                    {
                        Name = addCategoryModel.Name,
                        CategoryImage = base64ImageData,
                    };

                    var categoryEntity = new CategoryEntity
                    {
                        Name = careerDto.Name,
                        CategoryImage = careerDto.CategoryImage,
                    };

                    _dbContext.Categories.Add(categoryEntity);
                    await _dbContext.SaveChangesAsync();


                    return new OkObjectResult(new
                    {
                        message = "Category created successfully",
                        categoryEntity = categoryEntity
                    });
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = $"Internal server error: {ex.Message}" })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
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
            return _dbContext.Categories.Any(e => e.Name == name);
        }
    }
}

