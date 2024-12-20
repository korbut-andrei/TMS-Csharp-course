﻿using System.IdentityModel.Tokens.Jwt;

namespace AndreiKorbut.CareerChoiceBackend.Models.Auth
{
    public class AuthenticationResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
