using Final_project.Entities;
using Final_project.Entities.DbContexts;
using Final_project.Models.General;
using Final_project.Models.POST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;

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
                        CareerEntity = null,
                        ErrorMessage = $"You need to upload a career image."
                    };
                }

                // Validate file type
                if (!IsFileOfTypeValid(addCareerModel.CareerImage))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Invalid file type. Only JPEG or PNG files are allowed."
                    };
                }

                if (string.IsNullOrWhiteSpace(addCareerModel.Name))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Career name can't be empty."
                    };
                }
                if (string.IsNullOrWhiteSpace(addCareerModel.Description))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Career description can't be empty."
                    };
                }
                if (addCareerModel.CategoryId == 0)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Career categoryId can't be empty."
                    };
                }
                if (!CategoryExists(addCareerModel.CategoryId))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Provided career categoryId doesn't exist."
                    };
                }
                if (IsNameUnique(addCareerModel.Name))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Careeer with such name already exists."
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
                    };

                    _dbContext.Careers.Add(careerEntity);
                    await _dbContext.SaveChangesAsync();


                    // Process other fields in careerAddModel

                    return new CareerServiceResponseModel
                    {
                        Success = true,
                        CareerEntity = careerEntity,
                        ErrorMessage = $"Career has been successfully created."
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in AddCareer: {ex.Message}");
                return new CareerServiceResponseModel
                {
                    Success = false,
                    CareerEntity = null,
                    ErrorMessage = $"Internal server error: {ex.Message}"
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
                        CareerEntity = null,
                        ErrorMessage = $"Career with id {editCareerModel.Id} not found."
                    };
                }


                if (editCareerModel.CareerImage == null || editCareerModel.CareerImage.Length == 0)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"You need to upload a career image."
                    };
                }

                // Validate file type
                if (!IsFileOfTypeValid(editCareerModel.CareerImage))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Invalid file type. Only JPEG or PNG files are allowed."
                    };
                }

                if (string.IsNullOrWhiteSpace(editCareerModel.Name))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Career name can't be empty."
                    };
                }
                if (string.IsNullOrWhiteSpace(editCareerModel.Description))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Career description can't be empty."
                    };
                }
                if (editCareerModel.CategoryId == 0)
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Career categoryId can't be empty."
                    };
                }
                if (!CategoryExists(editCareerModel.CategoryId))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Provided career categoryId doesn't exist."
                    };
                }
                if (IsNameUnique(editCareerModel.Name))
                {
                    return new CareerServiceResponseModel
                    {
                        Success = false,
                        CareerEntity = null,
                        ErrorMessage = $"Careeer with such name already exists."
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
                        CareerEntity = careerEntity,
                        ErrorMessage = $"Career has been edited successfully."
                    };
                }
            }
            catch (Exception ex)
            {
                return new CareerServiceResponseModel
                {
                    Success = false,
                    CareerEntity = null,
                    ErrorMessage = $"Internal server error: {ex.Message}"
                };
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


