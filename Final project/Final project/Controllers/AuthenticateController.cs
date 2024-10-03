using Final_project.Entities;
using Final_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Final_project.Models.Auth;
using Final_project.Services;

namespace Final_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthenticateController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var result = await _authService.RegisterUserAsync(model);

            if (!result.Success)
            {

                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = $"{result.ServerMessage}" });
            }

            return Ok(new Response { Status = "Success", Message = $"{result.ServerMessage}" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _authService.Authenticate(model, model.UserAgent);

            SetRefreshTokenInCookie(response.RefreshToken);

            if (response.Success)
            {
                return Ok(new
                {
                    Status = "Success",
                    JwtToken = response.JwtToken,
                    ServerMessage = response.ServerMessage
                });
            }
            return StatusCode(StatusCodes.Status401Unauthorized,
                new Response { Status = "Error", Message = $"{response.ServerMessage}" });
        }

        [HttpPost]
        [Route("RefreshTokens")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Refresh token is missing." });
            }

            var response = await _authService.RefreshToken(refreshToken);

            if (!response.Success)
            {
                return StatusCode(StatusCodes.Status400BadRequest, 
                    new Response { Status = "Error", Message = $"{response.ServerMessage}" });
            }

            SetRefreshTokenInCookie(response.RefreshToken);

            return Ok(new {
                Status = "Success",
                JwtToken = response.JwtToken,
                ServerMessage = $"{response.ServerMessage}"
            });
        }

        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> LogOutAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Refresh token is missing." });
            }

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty (token))
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Jwt token is missing." });
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "User of given JWT couldn't be found." });
            }

            var result = await _authService.LogOutAsync(username, refreshToken);

            if (result.Success)
            {
                return Ok(new
                {
                    Status = "Success",
                    ServerMessage = $"{result.ServerMessage}"
                });
            }

            return StatusCode(StatusCodes.Status400BadRequest, 
                new Response { Status = "Error", Message = $"{result.ServerMessage}" });

        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        /*
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        */
    }
}
