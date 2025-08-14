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
        public DbSet<UsuarioLogin> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permiso> Permissions { get; set; }
        public DbSet<PermisoXRol> PermissionByRole { get; set; }
        public DbSet<UsuarioXRol> UserByRol { get; set; }
        public DbSet<ServicioEmail> EmailServices { get; set; }
        public DbSet<TipoEvento> TypesClasses { get; set; }
        public DbSet<ActivityByCoach> ActivityByCoach { get; set; }
        public DbSet<Modalidad> Modalities { get; set; }
        public DbSet<Class> Classes { get; set; }

        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
=> optionsBuilder.UseLoggerFactory(logger);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioXRol>(entity =>
            {
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();
            });
            base.OnModelCreating(modelBuilder);
            //llamo a los Configurations
            modelBuilder.ApplyConfiguration(new UserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermisoConfiguration());
            modelBuilder.ApplyConfiguration(new PermisoXRolConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioXRolConfiguration());
            modelBuilder.ApplyConfiguration(new ServicioEmailConfiguration());
            modelBuilder.ApplyConfiguration(new TipoEventoConfiguration());
            modelBuilder.ApplyConfiguration(new ActividadesXEntrenadorConfiguration());
            modelBuilder.ApplyConfiguration(new EventoConfiguration());
            modelBuilder.ApplyConfiguration(new ModalidadConfiguration());
        }
    }
}
