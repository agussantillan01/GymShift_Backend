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
            builder.ToTable("EmailService");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(e => e.EmailDescription).HasColumnName("emailDescription").HasMaxLength(500).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.emailSender).HasColumnName("emailSender").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.EmailReceptor).HasColumnName("emailReceptor").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.Subject).HasColumnName("Subject").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.Body).HasColumnName("body").HasMaxLength(250).IsUnicode(true).IsRequired(false);
            builder.Property(e => e.DateSent).HasColumnName("dateSent").IsRequired();

        }
    }
}
