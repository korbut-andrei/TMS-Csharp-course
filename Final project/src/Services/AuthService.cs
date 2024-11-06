using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Entities.DbContexts;
using AndreiKorbut.CareerChoiceBackend.Models.Auth;
using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.GETmodels;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public class AuthService
    {
        private readonly CareerContext _dbContext;

        private readonly DbRecordsCheckService _dbRecordsCheckService;

        private readonly ImageService _imageService;

        private readonly TokenService _tokenService;

        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(CareerContext dbContext, DbRecordsCheckService dbRecordsCheckService,
            ImageService imageService,
            UserManager<UserEntity> userManager,
            RoleManager<ApplicationRole> roleManager,
             TokenService tokenService,
            IConfiguration configuration
            )
        {
            _dbContext = dbContext;
            _dbRecordsCheckService = dbRecordsCheckService;
            _imageService = imageService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<UserResponseModel> RegisterUserAsync(RegisterModel model)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = new UserEntity
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errorMessages = result.Errors
                        .Select((error, index) => $"{index + 1}. {error.Description}");

                    return new UserResponseModel
                    {
                        Success = false,
                        ServerMessage = string.Join("; ", errorMessages),
                        UserId = 0
                    };
                }

                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    var imageResponse = await _imageService.AddImage(model.ProfileImage);
                    if (!imageResponse.Success)
                    {
                        return new UserResponseModel
                        {
                            Success = false,
                            ServerMessage = imageResponse.ServerMessage,
                            UserId = 0
                        };
                    }

                    user.Country = model.Country;
                    user.ImageId = imageResponse.ImageId;
                }

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new UserResponseModel
                {
                    Success = true,
                    UserId = user.Id,
                    ServerMessage = "User has been registered successfully."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new UserResponseModel
                {
                    Success = false,
                    UserId = 0,
                    ServerMessage = $"An error occurred while registering the user: {ex.Message}"
                };
            }
        }

        public async Task<AuthenticationResponseModel> Authenticate(LoginModel model, string userAgent)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null) 
            {
                return new AuthenticationResponseModel
                {
                    Success = false,
                    ServerMessage = $"Given user doesn't exist.",
                };
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new AuthenticationResponseModel
                {
                    Success = false,
                    ServerMessage = $"Password is incorrect.",
                };
            }

            var jwtToken = await _tokenService.GenerateJwtToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expiration = DateTime.Now.AddDays(7),
                UserId = user.Id,
                UserAgent = userAgent
            };

            _dbContext.RefreshTokens.Add(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponseModel
            {
                Success = true,
                ServerMessage = $"User has been successfully authenticated.",
                JwtToken = jwtToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthenticationResponseModel> RefreshToken(string refreshToken)
        {
            var retrievedRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (retrievedRefreshToken == null || retrievedRefreshToken.Expiration < DateTime.Now)
            {
                return new AuthenticationResponseModel
                {
                    Success = false,
                    ServerMessage = $"Refresh token is invalid or has expired.",
                    JwtToken = null,
                    RefreshToken = null
                };
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == retrievedRefreshToken.UserId);

            if (user == null)
            {
                return new AuthenticationResponseModel
                {
                    Success = false,
                    ServerMessage = $"User connected to the provided token has not been found.",
                    JwtToken = null,
                    RefreshToken = null
                };
            }

            var jwtToken = await _tokenService.GenerateJwtToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            retrievedRefreshToken.Token = newRefreshToken;

            retrievedRefreshToken.Expiration = DateTime.Now.AddDays(7);

            await _dbContext.SaveChangesAsync();

            return new AuthenticationResponseModel
            {
                Success = true,
                ServerMessage = $"Token has been successfully refreshed.",
                JwtToken = jwtToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<AuthenticationResponseModel> LogOutAsync(string username, string refreshToken)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return new AuthenticationResponseModel
                {
                    Success = false,
                    ServerMessage = "JWT's user doesn't exist."
                };
            }

            await _userManager.UpdateSecurityStampAsync(user);

            var tokenToRemove = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenToRemove != null) 
            {
                _dbContext.RefreshTokens.Remove(tokenToRemove);
                await _dbContext.SaveChangesAsync();
            }

            return new AuthenticationResponseModel
            {
                Success = true,
                ServerMessage = "User has been logged out successfully."
            };

        }

        /*
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        */
    }
}
