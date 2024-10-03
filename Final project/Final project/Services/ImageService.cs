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
using System.Net.Mime;
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
                if (image != null && image.Length > 0)
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
                        Base64ImageData = transformedImage,
                        //FileName = image.FileName
                    };

                    _dbContext.Images.Add(imageEntity);
                    await _dbContext.SaveChangesAsync(); // Save changes to get the generated Id

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
                return new ImageResponseModel
                {
                    Success = false,
                    ImageId = 0,    
                    ServerMessage = $"{ex.Message}"
                };
        }
}

        public async Task<ImageResponseModel> GetImage(int imageId)
        {
            try
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
            catch (Exception ex)
            {
                return new ImageResponseModel
                {
                    Success = false,
                    ImageId = 0,
                    Image = null,
                    ServerMessage = $"Error appeared during image extraction: {ex.Message}"
                };
            }
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

            try
            {
                //IFormFile formFile = null;
                
                byte[] JPG = { 255, 216, 255 };
                
                byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };

                byte[] fileBytes = Convert.FromBase64String(base64ImageData);
                
                string fileExtension = "";

                string contentType = "";

                if (fileBytes.Take(3).SequenceEqual(JPG))
                {
                    fileExtension = "jpeg";
                    contentType = "image/jpeg";

                }
                else if (fileBytes.Take(16).SequenceEqual(PNG))
                {
                    fileExtension = "png";
                    contentType = "image/png";

                }
                // Set default file extension if no match is found
                if (string.IsNullOrEmpty(fileExtension))
                {
                    fileExtension = "application/octet-stream";
                    contentType = "application/octet-stream";

                }

                // Generate a unique file name
                string fileName = $"CareerImage_{Guid.NewGuid()}.{fileExtension}";

                IHeaderDictionary headers = new HeaderDictionary();


                // Create a MemoryStream from the byte array
                using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                {
                    // Create an IFormFile
                    IFormFile  formFile = new FormFile(memoryStream, 0, memoryStream.Length, "CareerImage", fileName)
                    {
                        Headers = headers, // Assign the initialized headers to the FormFile object
                        ContentType = contentType
                    };

                    return formFile;
                }

            }
            catch (Exception ex) 
            {
                throw;
            }
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
