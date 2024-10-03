using Final_project.Models.General;
using Final_project.Models.POST;
using Microsoft.AspNetCore.Mvc;

namespace Final_project.Services
{
    public interface ICategoryService
    {
        Task<CategoryServiceResponseModel> AddCategory(AddCategoryModel addCategoryModel);
    }
}
