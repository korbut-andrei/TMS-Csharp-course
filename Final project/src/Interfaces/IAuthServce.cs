using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.Auth;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace CareerChoiceBackend.Interfaces
{
    public interface IAuthServce
    {

        public Task<UserResponseModel> RegisterUserAsync(RegisterModel model);

        public Task<AuthenticationResponseModel> Authenticate(LoginModel model, string userAgent);


        public Task<AuthenticationResponseModel> RefreshToken(string refreshToken);

        public Task<AuthenticationResponseModel> LogOutAsync(string username, string refreshToken);
    }
}
