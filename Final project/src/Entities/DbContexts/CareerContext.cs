using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using AndreiKorbut.CareerChoiceBackend.Entities;
using AndreiKorbut.CareerChoiceBackend.Models.Auth;


namespace AndreiKorbut.CareerChoiceBackend.Entities.DbContexts
{
    public class CareerContext : IdentityDbContext<UserEntity, ApplicationRole, int,
    ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
    ApplicationRoleClaim, ApplicationUserToken>
    {

        public CareerContext(DbContextOptions<CareerContext> options) : base(options)
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

            modelBuilder.Entity<CareerCharacteristicReviewEntity>()
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

            modelBuilder.Entity<ImageEntity>()
                .HasOne(e => e.Career)
                .WithOne(e => e.ImageEntity)
                .HasForeignKey<CareerEntity>(e => e.ImageId)
                .IsRequired();

            modelBuilder.Entity<CategoryEntity>()
                .HasOne(e => e.ImageEntity)
                .WithOne(e => e.Category)
                .HasForeignKey<CategoryEntity>(e => e.ImageId)
                .IsRequired();

            modelBuilder.Entity<ImageEntity>()
                .Property(e => e.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<RefreshToken>()
                .Property(e => e.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.UserEntity)
                .WithMany(ue => ue.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .IsRequired();

            // Configure ReviewEntity
            modelBuilder.Entity<ReviewEntity>()
                .Property(r => r.Rating)
                .HasColumnType("decimal(18,2)"); // Specify the precision and scale
            
            modelBuilder.Entity<ReviewBulletPointEntity>()
            .HasOne(rbp => rbp.ReviewEntity)
            .WithMany(r => r.ReviewBulletPoints)
            .HasForeignKey(rbp => rbp.ReviewEntityId)
            .OnDelete(DeleteBehavior.Restrict);

            // Configure CareerCharacteristicReviewEntity
            modelBuilder.Entity<CareerCharacteristicReviewEntity>()
                .Property(r => r.Rating)
                .HasColumnType("decimal(18,2)"); // Specify the precision and scale
        }

        public DbSet<CareerEntity> Careers { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<CareerCharacteristicEntity> Characteristics { get; set; }
        public DbSet<CareerCharacteristicReviewEntity> CharacteristicReviews { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<EducationOptionEntity> EducationOptions { get; set; }
        public DbSet<ReviewBulletPointEntity> ReviewBulletPoints { get; set; }
        public DbSet<SalaryReportEntity> SalaryReports { get; set; }
        public DbSet<TypicalTaskEntity> TypicalTasks { get; set; }
        public DbSet<UserEntity> UserRecords { get; set; }
        public DbSet<ImageEntity> Images { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
