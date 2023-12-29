using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Final_project.Entities.DbContexts
{
    public class CareerContext : DbContext
    {

        public CareerContext(DbContextOptions options) : base(options)
        {

        }

        public CareerContext( )
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReviewEntity>()
            .HasOne(e => e.UserEntity)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CareerEntity>()
                .HasOne(e => e.CategoryEntity)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReviewEntity>()
                .HasOne(e => e.CareerEntity)
                .WithMany()
                .HasForeignKey(e => e.CareerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CareerCharacteristicEntity>()
                .HasOne(e => e.CareerEntity)
                .WithMany()
                .HasForeignKey(e => e.CareerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CareerCharacteristicReviewEntity>()
                .HasOne(e => e.CareerCharacteristicEntity)
                .WithMany()
                .HasForeignKey(e => e.CareerCharacteristicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CareerCharacteristicReviewEntity>()
                .HasOne(e => e.UserEntity)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EducationOptionEntity>()
                .HasOne(e => e.CareerEntity)
                .WithMany()
                .HasForeignKey(e => e.CareerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReviewBulletPointEntity>()
                .HasOne(e => e.ReviewEntity)
                .WithMany()
                .HasForeignKey(e => e.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SalaryReportEntity>()
                .HasOne(e => e.CareerEntity)
                .WithMany()
                .HasForeignKey(e => e.CareerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TypicalTaskEntity>()
                .HasOne(e => e.CareerEntity)
                .WithMany()
                .HasForeignKey(e => e.CareerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EducationOptionEntity>()
                .Property(e => e.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CareerEntity>()
                .HasIndex(e => e.Name)
                .IsUnique();
            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(e => e.Name)
                .IsUnique();
            modelBuilder.Entity<CareerCharacteristicEntity>()
                .HasIndex(e => e.Name)
                .IsUnique();
        }

        public virtual DbSet<CareerEntity> Careers { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<CareerCharacteristicEntity> Characteristics { get; set; }
        public DbSet<CareerCharacteristicReviewEntity> CharacteristicReviews { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<EducationOptionEntity> EducationOptions { get; set; }
        public DbSet<ReviewBulletPointEntity> ReviewBulletPoints { get; set; }
        public DbSet<SalaryReportEntity> SalaryReports { get; set; }
        public DbSet<TypicalTaskEntity> TypicalTasks { get; set; }
        public DbSet<UserEntity> Users { get; set; }    
    }
}
