using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AndreiKorbut.CareerChoiceBackend.Models.POST
{
    public class CareerDto
    {
        public CareerEntity Career { get; set; }
        public decimal AverageRating { get; set; }
        public double AverageSalary { get; set; }
        public int TotalReviews { get; set; }
        public int MaxSalary { get; set; }
        public int MinSalary { get; set; }
        public string CategoryName { get; set; }
        public double AverageEducationTime { get; set; }
        public string CareerImage { get; set; }
    }
}
