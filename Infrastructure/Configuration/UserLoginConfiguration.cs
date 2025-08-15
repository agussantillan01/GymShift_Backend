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
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.firstName).HasColumnName("firstName").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.lastName).HasColumnName("lastName").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.PasswordHash).HasColumnName("PasswordHash").HasColumnType("nvarchar").IsRequired();
            builder.Property(x => x.UserName).HasColumnName("UserName").HasMaxLength(500).HasColumnType("nvarchar").IsRequired();
            builder.Property(x => x.NormalizedUserName).HasColumnName("NormalizedUserName").HasColumnType("nvarchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.ConcurrencyStamp).HasColumnName("ConcurrencyStamp").HasColumnType("nvarchar").IsRequired();
            builder.Property(x => x.Email).HasColumnName("Email").HasMaxLength(500).HasColumnType("nvarchar").IsRequired();
            builder.Property(x => x.NormalizedEmail).HasColumnName("NormalizedEmail").HasColumnType("nvarchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.SecurityStamp).HasColumnName("SecurityStamp").HasColumnType("nvarchar").IsRequired(false);
            builder.Property(x => x.isUserAdmin).HasColumnName("isUserAdmin").HasColumnType("bit").IsRequired();


            //builder.Ignore(c => c.NormalizedUserName);
            //builder.Ignore(c => c.AccessFailedCount);
            ////builder.Ignore(c => c.ConcurrencyStamp);

            builder.Ignore(c => c.EmailConfirmed);
            builder.Ignore(c => c.LockoutEnabled);
            builder.Ignore(c => c.LockoutEnd);

            //builder.Ignore(c => c.NormalizedEmail);
            builder.Ignore(c => c.PhoneNumber);
            builder.Ignore(c => c.PhoneNumberConfirmed);
            ////builder.Ignore(c => c.SecurityStamp);
            builder.Ignore(c => c.TwoFactorEnabled);
            builder.Ignore(c => c.AccessFailedCount);
            //builder.Ignore(c => c.Pass);
        }
    }
}
