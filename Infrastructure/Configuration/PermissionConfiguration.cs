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
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permisos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Type).HasColumnName("Tipo").HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Action).HasColumnName("Accion").HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasColumnName("Descripcion").HasColumnType("nvarchar").HasMaxLength(255).IsRequired();
            builder.Property(x => x.ClaimType).HasColumnName("ClaimType").HasColumnType("nvarchar").HasMaxLength(100).IsRequired();
            builder.Ignore(r => r.PermissionsByRole);
        }
    }
}
