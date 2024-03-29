﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace ProjectManagement.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Risk> Risks { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Milestone> Milestones { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<Comment>()
                .HasOne(c => c.Project)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectUser>()
            .HasKey(pu => new { pu.UserId, pu.ProjectId });

            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId);

            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId);

            builder.Entity<ProjectUser>()
                .Property(pu => pu.IsManager)
                .HasColumnType("bit");
        }
    }
}
