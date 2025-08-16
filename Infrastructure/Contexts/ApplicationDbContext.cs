using Infrastructure.Configuration;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ILoggerFactory logger;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory logger) : base(options)
        {
            this.logger = logger;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        #region DataSets
        public DbSet<UserLogin> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionByRole> PermissionByRole { get; set; }
        public DbSet<UserByRole> UserByRol { get; set; }
        public DbSet<EmailService> EmailServices { get; set; }
        public DbSet<Activity> TypesClasses { get; set; }
        public DbSet<ActivityByCoach> ActivityByCoach { get; set; }
        public DbSet<Modality> Modalities { get; set; }
        public DbSet<Class> Classes { get; set; }

        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
=> optionsBuilder.UseLoggerFactory(logger);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserByRole>(entity =>
            {
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();
            });
            base.OnModelCreating(modelBuilder);
            //llamo a los Configurations
            modelBuilder.ApplyConfiguration(new UserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PermissioByRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserByRoleConfiguration());
            modelBuilder.ApplyConfiguration(new EmailServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityConfiguration());
            modelBuilder.ApplyConfiguration(new ActivitiesByCoach());
            modelBuilder.ApplyConfiguration(new ClassConfiguration());
            modelBuilder.ApplyConfiguration(new ModalityConfiguration());
        }
    }
}
