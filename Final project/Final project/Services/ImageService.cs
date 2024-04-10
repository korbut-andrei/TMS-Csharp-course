using Final_project.Entities;
using Final_project.Entities.DbContexts;
using Final_project.Models.General;
using Final_project.Models.GET_models;
using Final_project.Models.GETmodels;
using Final_project.Models.POST;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Final_project.Services
{
    public class ImageService : IImageService
    {
        private readonly CareerContext _dbContext;

        public ImageService(CareerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ImageResponseModel> AddImage(IFormFile image)
        {
            try
            {
                if (image != null || image.Length > 0)
                {
                    // Validate file type
                    if (!IsFileOfTypeValid(image))
                    {
                        return new ImageResponseModel
                        {
                            Success = false,
                            ImageId = 0,
                            ServerMessage = $"Invalid file type. Only JPEG or PNG files are allowed."
                        };
                    }

                    var transformedImage = await TransformIFileToB64String(image);

                        var imageEntity = new ImageEntity
                        {
                            Base64ImageData = transformedImage
                        };

                    _dbContext.Images.Add(imageEntity);
                    await _dbContext.SaveChangesAsync();

                    return new ImageResponseModel
                    {
                        Success = true,
                        ImageId = imageEntity.Id,
                        ServerMessage = $"Image has been successfully created."
                    };

                }
                return new ImageResponseModel
                {
                    Success = false,
                    ImageId = 0,
                    ServerMessage = "No image provided."
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in AddCareer: {ex.Message}");
                return new ImageResponseModel
                {
                    Success = false,
                    ImageId = 0,
                    ServerMessage = $"Internal server error: {ex.Message}"
                };
            
        }
}

        public async Task<ImageResponseModel> GetImage(int imageId)
        {
            var image = await _dbContext.Images.FindAsync(imageId);

            if (image == null)
            {
                return new ImageResponseModel
                {
                    Success = false,
                    ImageId = 0,
                    ServerMessage = $"Image with id {imageId} not found."
                };
            }

            var imageFile = await TransformB64StringToIFile(image.Base64ImageData);

            return new ImageResponseModel
            {
                Success = true,
                ImageId = imageId,
                Image = imageFile,
                ServerMessage = $"Image {imageId} has been successfully found."
            };
        }

        public async Task<ImageResponseModel> DeleteImage(int imageId)
        {
            try
            {
                // Check if the provided careerId is valid
                var requestedImage = await _dbContext.Images.FindAsync(imageId);

                if (requestedImage == null)
                {
                    return new ImageResponseModel
                    {
                        Success = false,
                        Image = null,
                        ServerMessage = $"Image with id {imageId} not found."
                    };
                }

                _dbContext.Images.Remove(requestedImage);

                await _dbContext.SaveChangesAsync();

                return new ImageResponseModel
                {
                    Success = true,
                    Image = null,
                    ImageId = imageId,
                    ServerMessage = $"Image has been successfully deleted."
                };
                }
            catch (Exception ex)
            {
                return new ImageResponseModel
                {
                    Success = false,
                    Image = null,
                    ImageId = imageId,
                    ServerMessage = $"Internal server error: {ex.Message}"
                };
            }
        }
             
        private async Task<string> TransformIFileToB64String(IFormFile imageFile)
        {
            using (var stream = new MemoryStream())
            {
                imageFile.CopyTo(stream);
                string base64ImageData = Convert.ToBase64String(stream.ToArray());

                return base64ImageData;
            }
        }

        private async Task<IFormFile> TransformB64StringToIFile(string base64ImageData)
        {

            IFormFile formFile = null;

            // Decode the Base64 string into a byte array
            byte[] fileBytes = Convert.FromBase64String(base64ImageData);

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                // Create an IFormFile
                formFile = new FormFile(memoryStream, 0, memoryStream.Length, "CareerImage", "CareerImage.jpg");

                // Now you can use the 'formFile' as needed, for example, save it to disk, process it, etc.
            }

            return formFile;
        }


        private bool IsFileOfTypeValid(IFormFile file)
        {
            // Add logic to check if the file type is JPEG or PNG
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(extension);
        }
    }
}
