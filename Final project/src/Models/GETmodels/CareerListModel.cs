﻿using AndreiKorbut.CareerChoiceBackend.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Models.GET_models
{
    public class CareerListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SalaryRange SalaryRange { get; set; }
        public string CategoryName { get; set; }
        public IFormFile CareerImage { get; set; }
        public AverageReviewRatingAndReviewCount AverageReviewAndReviewCount { get; set; }
        public ParameterValues[]? ParameterValues { get; set; }
    }
}
