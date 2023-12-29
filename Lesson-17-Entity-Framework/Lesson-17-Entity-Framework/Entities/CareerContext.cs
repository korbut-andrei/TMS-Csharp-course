﻿using Microsoft.EntityFrameworkCore;

namespace Lesson_17_Entity_Framework.Entities
{
    public class CareerContext : DbContext
    {
        public CareerContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<CareerEntity> Professions { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
    }
}
