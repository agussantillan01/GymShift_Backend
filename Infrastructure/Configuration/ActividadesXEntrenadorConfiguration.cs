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
    public class ActividadesXEntrenadorConfiguration : IEntityTypeConfiguration<ActividadesXEntrenador>
    {
        public void Configure(EntityTypeBuilder<ActividadesXEntrenador> builder)
        {
            builder.ToTable("ACTIVIDADESXCOACHS");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.IdUsuario)
                .HasColumnName("IDUSUARIO")
                .IsRequired();

            builder.Property(a => a.IdActividad)
                .HasColumnName("IDACTIVIDAD")
                .IsRequired();
        }
    
    }
}
