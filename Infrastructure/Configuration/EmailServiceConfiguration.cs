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
    public class EmailServiceConfiguration : IEntityTypeConfiguration<EmailService>
    {
        public void Configure(EntityTypeBuilder<EmailService> builder)
        {
            builder.ToTable("SERVICIOSEMAIL");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
            builder.Property(e => e.EmailDescription).HasColumnName("DESCRIPCIONEMAIL").HasMaxLength(500).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.emailSender).HasColumnName("EMAILEMISOR").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.EmailReceptor).HasColumnName("EMAILRECEPTOR").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.Subject).HasColumnName("ASUNTO").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.Body).HasColumnName("CUERPO").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.DateSent).HasColumnName("FECHAENVIO").IsRequired();

        }
    }
}
