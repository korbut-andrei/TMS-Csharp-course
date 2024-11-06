using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using Microsoft.AspNetCore.Mvc;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public interface ICategoryService
    {
        Task<CategoryServiceResponseModel> AddCategory(AddCategoryModel addCategoryModel);
    }
}
