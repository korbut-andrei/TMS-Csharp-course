using Final_project.Entities.DbContexts;
using Final_project.Models.Auth;
using Final_project.Models.POST;
using Final_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Final_project.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoryController
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CareerContext careerContext, CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory([FromForm] AddCategoryModel addCategoryModel)
        {
            var result = await _categoryService.AddCategory(addCategoryModel);

            if (result.Success)
            {
                return new OkObjectResult(new
                {
                    success = true,
                    message = result.ServerMessage,
                    Category = result.Category
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
    }
}
