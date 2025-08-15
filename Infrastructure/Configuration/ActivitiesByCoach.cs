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
    public class ActivitiesByCoach : IEntityTypeConfiguration<ActivityByCoach>
    {
        public void Configure(EntityTypeBuilder<ActivityByCoach> builder)
        {
            builder.ToTable("activitiesByCoachs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.IdUser)
                .HasColumnName("idUser")
                .IsRequired();

            builder.Property(a => a.IdActivity)
                .HasColumnName("idActivity")
                .IsRequired();
        }
    
    }
}
