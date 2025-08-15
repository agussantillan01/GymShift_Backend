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
    public class ModalityConfiguration : IEntityTypeConfiguration<Modality>
    {
        public void Configure(EntityTypeBuilder<Modality> builder)
        {
            builder.ToTable("MODALIDADES");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.modality).HasColumnName("MODALIDAD").HasColumnType("varchar").HasMaxLength(500).IsRequired();
        }
    }
}
