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
    public class PermissioByRoleConfiguration : IEntityTypeConfiguration<PermissionByRole>
    {
        public void Configure(EntityTypeBuilder<PermissionByRole> builder)
        {
            builder.ToTable("PermissionsByRole");
            builder.HasKey(pr => new { pr.IdRol, pr.IdPermission });

            //builder.HasOne(pr => pr.Rol)
                           //.WithMany(r => r.permisosXrol) // Un Rol tiene muchos PermisosXRol
                           //.HasForeignKey(pr => pr.IdRol) // Clave foránea
                           //.OnDelete(DeleteBehavior.Cascade);

            // Relación uno a muchos entre PermisoXRol y Permiso
            builder.HasOne(pr => pr.Permission)
                   .WithMany(p => p.PermissionsByRole) // Un Permiso tiene muchos PermisosXRol
                   .HasForeignKey(pr => pr.IdPermission) // Clave foránea
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
