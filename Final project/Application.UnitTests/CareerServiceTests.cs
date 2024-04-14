using Final_project.Entities.DbContexts;
using Final_project.Models.POST;
using Final_project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Final_project.Entities;
using Moq.EntityFrameworkCore;

namespace Application.UnitTests
{
}
      /*
    public class CareerServiceTests
    {
      
        [Fact]
        public async Task AddCareer_InvalidFileType_ReturnsError()
        {
            // Arrange
            // Create in-memory database options
            var dbContextOptions = new DbContextOptionsBuilder<CareerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Create a mock for CareerContext using the in-memory database options
            var mockDbContext = new Mock<CareerContext>(dbContextOptions);

            // Create some mock data for the DbSet
            var mockData = new List<CareerEntity>(); // Add any mock data you need

            // Setup DbSet to return mock data
            mockDbContext.Setup(x => x.Careers).ReturnsDbSet(mockData);

            // Initialize CareerService with the mock CareerContext
            var careerService = new CareerService(mockDbContext.Object);

            // Simulate content of a file with a different extension (e.g., .txt)
            var invalidFileType = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 };

            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.Length).Returns(invalidFileType.Length);
            formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(invalidFileType));

            var addCareerModel = new AddCareerModel
            {
                Name = "Test Career",
                Description = "Test Description",
                CategoryId = 1,
                CareerImage = formFileMock.Object
            };

            // Act
            var result = await careerService.AddCareer(addCareerModel);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Career);
            Assert.Contains("Invalid file type. Only JPEG or PNG files are allowed.", result.ServerMessage, StringComparison.OrdinalIgnoreCase);
        }

    }
}







    /*
// Arrange
            var dbContextOptions = new DbContextOptionsBuilder<CareerContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
var mockDbContext = new Mock<CareerContext>();
var careerService = new CareerService(mockDbContext.Object);

// Simulate content of a file with a different extension (e.g., .txt)
var invalidFileType = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 };

var formFileMock = new Mock<IFormFile>();
formFileMock.Setup(f => f.Length).Returns(invalidFileType.Length);
formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(invalidFileType));

var addCareerModel = new AddCareerModel
{
    Name = "Test Career",
    Description = "Test Description",
    CategoryId = 1,
    CareerImage = formFileMock.Object
};

// Act
var result = await careerService.AddCareer(addCareerModel);

// Assert
Assert.False(result.Success);
Assert.Null(result.CareerEntity);
Assert.Contains("Invalid file type. Only JPEG or PNG files are allowed.", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    */