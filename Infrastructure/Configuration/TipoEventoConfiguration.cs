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
    public class TipoEventoConfiguration : IEntityTypeConfiguration<TipoEvento>
    {
        public void Configure(EntityTypeBuilder<TipoEvento> builder)
        {
            builder.ToTable("TIPOSEVENTOS");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre).HasColumnName("TIPO").HasColumnType("varchar").HasMaxLength(500).IsRequired();


        }
    }
}
