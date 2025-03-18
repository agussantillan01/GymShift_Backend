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
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("EVENTOS");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IdTipoEvento).HasColumnName("IDTIPOEVENTO").HasColumnType("int").IsRequired();
            builder.Property(x => x.FechaInicio).HasColumnName("FECHAINICIO").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.FechaFin).HasColumnName("FECHAFIN").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Horario).HasColumnName("HORARIO").HasColumnType("Varchar").HasMaxLength(5).IsRequired();
            builder.Property(x => x.Duracion).HasColumnName("DURACION").HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Dias).HasColumnName("DIAS").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.IdModalidad).HasColumnName("IDMODALIDAD").HasColumnType("int").IsRequired();
            builder.Property(x => x.Valor).HasColumnName("VALOR").HasColumnType("money").IsRequired();
            builder.Property(x => x.Descripcion).HasColumnName("DESCRIPCION").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.CupoMaximo).HasColumnName("CUPOMAXIMO").HasColumnType("int").IsRequired();
            builder.Property(x => x.CupoActual).HasColumnName("CUPODEMOMENTO").HasColumnType("int").IsRequired();
            builder.Property(x => x.IdProfesor).HasColumnName("IDUSUARIO").HasColumnType("int").IsRequired();

        }
    }
}
