using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class PermisoXRolConfiguration : IEntityTypeConfiguration<PermisoXRol>
    {
        public void Configure(EntityTypeBuilder<PermisoXRol> builder)
        {
            builder.ToTable("PermisosXRol");
            builder.HasKey(pr => new { pr.IdRol, pr.IdPermiso });

            //builder.HasOne(pr => pr.Rol)
                           //.WithMany(r => r.permisosXrol) // Un Rol tiene muchos PermisosXRol
                           //.HasForeignKey(pr => pr.IdRol) // Clave foránea
                           //.OnDelete(DeleteBehavior.Cascade);

            // Relación uno a muchos entre PermisoXRol y Permiso
            builder.HasOne(pr => pr.Permiso)
                   .WithMany(p => p.PermisosXRol) // Un Permiso tiene muchos PermisosXRol
                   .HasForeignKey(pr => pr.IdPermiso) // Clave foránea
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
