﻿using System.ComponentModel.DataAnnotations;

namespace Lesson_21_JWT_Authentication.Models
{
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
}


public class LoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}