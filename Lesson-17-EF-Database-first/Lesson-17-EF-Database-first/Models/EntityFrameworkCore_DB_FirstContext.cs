using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lesson_17_EF_Database_first.Models
{
    public partial class EntityFrameworkCore_DB_FirstContext : DbContext
    {
        public EntityFrameworkCore_DB_FirstContext()
        {
        }

        public EntityFrameworkCore_DB_FirstContext(DbContextOptions<EntityFrameworkCore_DB_FirstContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectResource> ProjectResources { get; set; } = null!;
        public virtual DbSet<ProjectRole> ProjectRoles { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgrammingLanguage>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Programm__737584F6CE62AD73")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.HasIndex(e => e.Name, "UQ__Project__737584F6B0CD5B23")
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<ProjectResource>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ProjectRe__UserI__46E78A0C");

                entity.HasOne(d => d.ProjectRole)
                    .WithMany()
                    .HasForeignKey(d => new { d.ProjectId, d.RoleId })
                    .HasConstraintName("FK__ProjectResources__47DBAE45");
            });

            modelBuilder.Entity<ProjectRole>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.RoleId })
                    .HasName("PK__ProjectR__CEB512113DA233AF");

                entity.HasOne(d => d.ProgrammingLanguage)
                    .WithMany(p => p.ProjectRoles)
                    .HasForeignKey(d => d.ProgrammingLanguageId)
                    .HasConstraintName("FK__ProjectRo__Progr__44FF419A");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectRoles)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectRo__Proje__4316F928");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ProjectRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectRo__RoleI__440B1D61");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Roles__737584F61DD83A9D")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Nickname, "UQ__User__CC6CD17EA3FCF5FB")
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Nickname).HasMaxLength(200);

                entity.Property(e => e.PhotoUrl)
                    .HasMaxLength(300)
                    .HasColumnName("PhotoURL");

                entity.Property(e => e.Surname).HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
