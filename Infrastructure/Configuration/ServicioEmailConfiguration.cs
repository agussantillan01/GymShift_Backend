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
    public class ServicioEmailConfiguration : IEntityTypeConfiguration<ServicioEmail>
    {
        public void Configure(EntityTypeBuilder<ServicioEmail> builder)
        {
            builder.ToTable("SERVICIOSEMAIL");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
            builder.Property(e => e.DescripcionEmail).HasColumnName("DESCRIPCIONEMAIL").HasMaxLength(500).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.EmailEmisor).HasColumnName("EMAILEMISOR").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.EmailReceptor).HasColumnName("EMAILRECEPTOR").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.Asunto).HasColumnName("ASUNTO").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.Cuerpo).HasColumnName("CUERPO").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.FechaEnvio).HasColumnName("FECHAENVIO").IsRequired();

        }
    }
}
