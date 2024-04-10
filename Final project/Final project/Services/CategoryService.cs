using Final_project.Entities.DbContexts;
using Final_project.Entities;
using Final_project.Models.POST;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Final_project.Models.General;

namespace Final_project.Services
{
    public class CategoryService : ICategoryService
    {
        public List<CategoryEntity> Categories { get; } = new List<CategoryEntity>();

        private readonly CareerContext _dbContext;

        private readonly ImageService _imageService;

        private readonly DbRecordsCheckService _imageDbRecordsCheckService;

        public CategoryService(CareerContext dbContext, ImageService imageService, DbRecordsCheckService imageDbRecordsCheckService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _imageDbRecordsCheckService = imageDbRecordsCheckService;
        }

        public async Task<IActionResult> AddCategory(AddCategoryModel addCategoryModel)
        {
            try
            {
                if (addCategoryModel.CategoryImage == null || addCategoryModel.CategoryImage.Length == 0)
                {
                    return new BadRequestObjectResult("You need to upload a Category image");
                }

                if (string.IsNullOrWhiteSpace(addCategoryModel.Name))
                {
                    return new BadRequestObjectResult("Career name can't be empty.");
                }
                if (_imageDbRecordsCheckService.RecordExistsInDatabase(addCategoryModel.Name, "Categories", "Name"))
                {
                    return new BadRequestObjectResult("Category with such name already exists.");
                }

                var imageData = await _imageService.AddImage(addCategoryModel.CategoryImage);

                if (!imageData.Success)
                {
                    return new ObjectResult(new { error = $"Internal server error: {imageData.ServerMessage}" })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }

                var categoryEntity = new CategoryEntity
                    {
                        Name = addCategoryModel.Name,
                        ImageId = imageData.ImageId,
                    };

                    _dbContext.Categories.Add(categoryEntity);
                    await _dbContext.SaveChangesAsync();


                    return new OkObjectResult(new
                    {
                        message = "Category created successfully",
                        categoryEntity = categoryEntity
                    });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = $"Internal server error: {ex.Message}" })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}

