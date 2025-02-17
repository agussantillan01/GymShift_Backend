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
    public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.ToTable("Permisos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Tipo).HasColumnName("Tipo").HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Accion).HasColumnName("Accion").HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Descripcion).HasColumnName("Descripcion").HasColumnType("nvarchar").HasMaxLength(255).IsRequired();
            builder.Property(x => x.ClaimType).HasColumnName("ClaimType").HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            builder.Ignore(r => r.PermisosXRol);
        }
    }
}
