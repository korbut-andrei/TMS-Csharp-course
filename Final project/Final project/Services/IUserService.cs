using Final_project.Models.Auth;
using Final_project.Models.GETmodels;
using Final_project.Models.POST;
using Microsoft.AspNetCore.Identity;

namespace Final_project.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    }
}
