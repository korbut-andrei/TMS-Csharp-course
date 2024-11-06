using AndreiKorbut.CareerChoiceBackend.Models.Auth;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using Microsoft.AspNetCore.Identity;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> RegisterUserAsync(RegisterModel model);
    }
}
