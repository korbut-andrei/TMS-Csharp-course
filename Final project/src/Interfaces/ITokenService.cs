using AndreiKorbut.CareerChoiceBackend.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CareerChoiceBackend.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateJwtToken(UserEntity user);

        public string GenerateRefreshToken();
    }
}
