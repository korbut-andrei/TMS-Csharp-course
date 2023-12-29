using Final_project.Models.POST;
using Microsoft.AspNetCore.Mvc;

namespace Final_project.Services
{
    public interface ICategoryService
    {
        Task<IActionResult> AddCategory(AddCategoryModel addCategoryModel);
    }
}
