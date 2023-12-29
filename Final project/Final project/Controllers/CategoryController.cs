using Final_project.Entities.DbContexts;
using Final_project.Models.Auth;
using Final_project.Models.POST;
using Final_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCareer([FromForm] AddCategoryModel addCategoryModel)
        {
            return await _categoryService.AddCategory(addCategoryModel);
        }
    }
}
