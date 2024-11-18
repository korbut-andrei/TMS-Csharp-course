using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AndreiKorbut.CareerChoiceBackend.Models.General;
using CareerChoiceBackend.Interfaces;
using CareerChoiceBackend.Entities;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public class CategoryService : ICategoryService
    {
        public List<CategoryEntity> Categories { get; } = new List<CategoryEntity>();

        private readonly CareerContext _dbContext;

        private readonly IImageService _imageService;

        private readonly IDbRecordsCheckService _imageDbRecordsCheckService;

        public CategoryService(CareerContext dbContext, IImageService imageService, IDbRecordsCheckService imageDbRecordsCheckService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _imageDbRecordsCheckService = imageDbRecordsCheckService;
        }

        public async Task<CategoryServiceResponseModel> AddCategory(AddCategoryModel addCategoryModel)
        {
            try
            {
                if (addCategoryModel.CategoryImage == null || addCategoryModel.CategoryImage.Length == 0)
                {
                    return new CategoryServiceResponseModel
                    {
                        Success = false,
                        Category = null,
                        ServerMessage = $"You need to upload a Category image."
                    };
                }

                if (string.IsNullOrWhiteSpace(addCategoryModel.Name))
                {
                    return new CategoryServiceResponseModel
                    {
                        Success = false,
                        Category = null,
                        ServerMessage = $"Career name can't be empty."
                    };
                }
                if (_imageDbRecordsCheckService.RecordExistsInDatabase(addCategoryModel.Name, "Categories", "Name"))
                {
                    return new CategoryServiceResponseModel
                    {
                        Success = false,
                        Category = null,
                        ServerMessage = $"Category with such name already exists."
                    };
                }

                var imageData = await _imageService.AddImage(addCategoryModel.CategoryImage);

                if (!imageData.Success)
                {
                    return new CategoryServiceResponseModel
                    {
                        Success = false,
                        Category = null,
                        ServerMessage = $"{imageData.ServerMessage}"
                    };
                }

                var categoryEntity = new CategoryEntity
                    {
                        Name = addCategoryModel.Name,
                        ImageId = imageData.ImageId,
                    };

                    _dbContext.Categories.Add(categoryEntity);
                    await _dbContext.SaveChangesAsync();

                return new CategoryServiceResponseModel
                {
                    Success = true,
                    Category = categoryEntity,
                    ServerMessage = $"Category has been created successfully."
                };
            }
            catch (Exception ex)
            {
                return new CategoryServiceResponseModel
                {
                    Success = false,
                    Category = null,
                    ServerMessage = $"Internal server error: {ex.Message}"
                };
            }
        }
    }
}

