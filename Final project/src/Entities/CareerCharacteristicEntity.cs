using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace AndreiKorbut.CareerChoiceBackend.Entities
{
    public class CareerCharacteristicEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        public string Type { get; set; }
    }
}






