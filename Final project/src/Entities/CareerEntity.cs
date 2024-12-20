﻿using AndreiKorbut.CareerChoiceBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AndreiKorbut.CareerChoiceBackend.Entities
{
    public class CareerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual CategoryEntity CategoryEntity { get; set; }

        public bool IsDeleted { get; set; }

        //[ForeignKey("ImageId")]
        public int ImageId { get; set; }
        public virtual ImageEntity ImageEntity { get; set; }
    }
}
