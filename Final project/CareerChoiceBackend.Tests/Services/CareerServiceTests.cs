using Moq;
using Xunit;
using System.Threading.Tasks;
using AndreiKorbut.CareerChoiceBackend.Models;
using AndreiKorbut.CareerChoiceBackend.Services;
using AndreiKorbut.CareerChoiceBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using CareerChoiceBackend.Entities;
using CareerChoiceBackend.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using CareerChoiceBackend.Tests.MockObjects;
using Microsoft.AspNetCore.Http;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;


namespace CareerChoiceBackend.Tests.Services
{
    public class CareerServiceTests
    {
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<IDbRecordsCheckService> _mockDbRecordsCheckService;
        private readonly Mock<IHashHelper> _mockHashHelper;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<ILogger<CareerService>> _mockLogger;
        private readonly Mock<CareerContext> _mockDbContext;

        private readonly CareerService _careerService;

        public CareerServiceTests()
        {
            _mockImageService = new Mock<IImageService>();
            _mockDbRecordsCheckService = new Mock<IDbRecordsCheckService>();
            _mockHashHelper = new Mock<IHashHelper>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockLogger = new Mock<ILogger<CareerService>>();
            _mockDbContext = new Mock<CareerContext>(new Microsoft.EntityFrameworkCore.DbContextOptions<CareerContext>());

            _careerService = new CareerService(
                _mockDbContext.Object,
                _mockImageService.Object,
                _mockDbRecordsCheckService.Object,
                _mockHashHelper.Object,
                _mockMemoryCache.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task AddCareer_MissingCareerImage_ReturnsError()
        {
            // Arrange
            var addCareerModel = new AddCareerModel
            {
                CareerImage = null,
                Name = "Test Career",
                Description = "Test Description",
                CategoryId = 1
            };

            // Act
            var result = await _careerService.AddCareer(addCareerModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("You need to upload a career image.", result.ServerMessage);
        }

        [Fact]
        public async Task AddCareer_InvalidCategoryId_ReturnsError()
        {
            // Arrange
            var careerImageData = new byte[] { 1, 2, 3 };
            IFormFile careerImage = new FormFileMock(careerImageData, "careerImage.jpg");

            var addCareerModel = new AddCareerModel
            {
                CareerImage = careerImage,
                Name = "New Career",
                Description = "Career Description",
                CategoryId = 999
            };

            _mockDbRecordsCheckService
                .Setup(x => x.RecordExistsInDatabase(addCareerModel.CategoryId, "Categories", "Id"))
                .Returns(false);

            // Act
            var result = await _careerService.AddCareer(addCareerModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Provided categoryId doesn't exist.", result.ServerMessage);
        }

        [Fact]
        public async Task AddCareer_DuplicateCareerName_ReturnsError()
        {
            // Arrange
            var careerImageData = new byte[] { 1, 2, 3 };
            IFormFile careerImage = new FormFileMock(careerImageData, "careerImage.jpg");

            var addCareerModel = new AddCareerModel
            {
                CareerImage = careerImage,
                Name = "Existing Career",
                Description = "Career Description",
                CategoryId = 1
            };

            _mockDbRecordsCheckService
                .Setup(x => x.RecordExistsInDatabase(addCareerModel.CategoryId, "Categories", "Id"))
                .Returns(true);
            _mockDbRecordsCheckService
                .Setup(x => x.RecordExistsInDatabase(addCareerModel.Name, "Careers", "Name"))
                .Returns(true);

            // Act
            var result = await _careerService.AddCareer(addCareerModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Careeer with such name already exists.", result.ServerMessage);
        }

        [Fact]
        public async Task AddCareer_ValidData_ReturnsSuccess()
        {
            // Arrange
            var careerImageData = new byte[] { 1, 2, 3 };
            IFormFile careerImage = new FormFileMock(careerImageData, "careerImage.jpg");

            var addCareerModel = new AddCareerModel
            {
                CareerImage = careerImage,
                Name = "New Career",
                Description = "Career Description",
                CategoryId = 1
            };

            _mockDbRecordsCheckService
                .Setup(x => x.RecordExistsInDatabase(addCareerModel.CategoryId, "Categories", "Id"))
                .Returns(true); // Category exists
            _mockDbRecordsCheckService
                .Setup(x => x.RecordExistsInDatabase(addCareerModel.Name, "Careers", "Name"))
                .Returns(false); // No existing career with this name
            _mockImageService
                .Setup(x => x.AddImage(addCareerModel.CareerImage))
                .ReturnsAsync(new ImageResponseModel { Success = true, ImageId = 1 });

            // Mock data for Careers DbSet
            var careerEntities = new List<CareerEntity>
            {
                new CareerEntity { Id = 1, Name = "Test Career", Description = "Test Description" }
            }.AsQueryable();

            // Create a mock DbSet
            var mockSet = new Mock<DbSet<CareerEntity>>();

            // Mock IQueryable behavior for the DbSet
            mockSet.As<IQueryable<CareerEntity>>()
                .Setup(m => m.Provider)
                .Returns(careerEntities.Provider);
            mockSet.As<IQueryable<CareerEntity>>()
                .Setup(m => m.Expression)
                .Returns(careerEntities.Expression);
            mockSet.As<IQueryable<CareerEntity>>()
                .Setup(m => m.ElementType)
                .Returns(careerEntities.ElementType);
            mockSet.As<IQueryable<CareerEntity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(careerEntities.GetEnumerator());

            // Mock Add method on DbSet
            mockSet.Setup(m => m.Add(It.IsAny<CareerEntity>())).Verifiable();

            // Mock the CareerContext to return the mocked DbSet
            _mockDbContext.Setup(m => m.Careers).Returns(mockSet.Object);

            // Mock SaveChangesAsync on DbContext
            _mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _careerService.AddCareer(addCareerModel);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Career has been successfully created.", result.ServerMessage);
        }
    }
}
