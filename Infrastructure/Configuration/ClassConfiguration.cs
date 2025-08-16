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
            builder.ToTable("Classes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DateFrom).HasColumnName("hourFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.DateTo).HasColumnName("hourTo").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Schedule).HasColumnName("schedules").HasColumnType("Varchar").HasMaxLength(5).IsRequired();
            builder.Property(x => x.Duration).HasColumnName("duratiom").HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Days).HasColumnName("days").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.Price).HasColumnName("price").HasColumnType("money").IsRequired();
            builder.Property(x => x.Description).HasColumnName("descriiption").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.AmountMax).HasColumnName("amountMax").HasColumnType("int").IsRequired();
            builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("int").IsRequired();
            builder.Property(x => x.idModality).HasColumnName("idModality").HasColumnType("int").IsRequired();
            builder.Property(x => x.IdCoach).HasColumnName("idUser").HasColumnType("int").IsRequired();
            builder.Property(x => x.ApplicationStatus).HasColumnName("idApplicationStatus").HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.idActivity).HasColumnName("idActivity").HasColumnType("int").IsRequired();

        }
    }
}
