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
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("EVENTOS");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.idTypeClass).HasColumnName("IDTIPOEVENTO").HasColumnType("int").IsRequired();
            builder.Property(x => x.DateFrom).HasColumnName("FECHAINICIO").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.DateTo).HasColumnName("FECHAFIN").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Schedule).HasColumnName("HORARIO").HasColumnType("Varchar").HasMaxLength(5).IsRequired();
            builder.Property(x => x.Duration).HasColumnName("DURACION").HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Days).HasColumnName("DIAS").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.IdModality).HasColumnName("IDMODALIDAD").HasColumnType("int").IsRequired();
            builder.Property(x => x.Price).HasColumnName("VALOR").HasColumnType("money").IsRequired();
            builder.Property(x => x.Description).HasColumnName("DESCRIPCION").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.AmountMax).HasColumnName("CUPOMAXIMO").HasColumnType("int").IsRequired();
            builder.Property(x => x.Amount).HasColumnName("CUPODEMOMENTO").HasColumnType("int").IsRequired();
            builder.Property(x => x.IdCoach).HasColumnName("IDUSUARIO").HasColumnType("int").IsRequired();
            builder.Property(x => x.ApplicationStatus).HasColumnName("ESTADOSOLICITUD").HasColumnType("varchar").HasMaxLength(100);

        }
    }
}
