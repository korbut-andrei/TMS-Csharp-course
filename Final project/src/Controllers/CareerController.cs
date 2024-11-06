using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Entities.DbContexts;
using AndreiKorbut.CareerChoiceBackend.Enums;
using AndreiKorbut.CareerChoiceBackend.Models;
using AndreiKorbut.CareerChoiceBackend.Models.Auth;
using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using AndreiKorbut.CareerChoiceBackend.Models.QueryModels;
using AndreiKorbut.CareerChoiceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AndreiKorbut.CareerChoiceBackend.Controllers
{
    [Route("api/Careers")]
    [ApiController]
    public class CareerController : ControllerBase
    {
        private readonly CareerService _careerService;

        private readonly ILogger<CareerController> _logger;

        public CareerController(CareerService careerService, ILogger<CareerController> logger)
        {
            _careerService = careerService;
            _logger = logger;
        }

        //[Authorize(Roles = UserRoles.Admin)]
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
        [Route("DeleteCareer")]
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

        
        [HttpGet]
        [Route("GetListOfCareers")]
        public async Task<IActionResult> GetListOfCareers([FromQuery]
        [Required] GetCareersListQueryModel queryParameters
        )
        {
            HttpContext.Request.EnableBuffering();

            _logger.LogInformation("Request to GetListOfCareers started. IP: {IP}, Parameters: {@queryParameters}", 
                HttpContext.Connection.RemoteIpAddress, queryParameters);

            using (var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = await reader.ReadToEndAsync();
                _logger.LogInformation("Request Body: {Body}", body);

                HttpContext.Request.Body.Position = 0;
            }

            try
            {
                var result = await _careerService.GetListOfCareers(queryParameters);

                if (result.Success)
                {
                    _logger.LogInformation("GetListOfCareers successfully completed.");

                    return new OkObjectResult(new
                    {
                        message = result.ServerMessage,
                        careers = result.Careers,
                        totalPages = result.TotalPages
                    });
                }
                else
                {
                    _logger.LogError("GetListOfCareers failed. Error: {Error}", result.ServerMessage);

                    return new ObjectResult(new { error = result.ServerMessage })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unhandled error occurred during GetListOfCareers.");

                return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "An unexpected error occurred." });
            }

        }
    }
}
