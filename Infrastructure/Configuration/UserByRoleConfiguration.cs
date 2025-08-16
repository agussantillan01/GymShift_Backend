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
    public class UserByRoleConfiguration : IEntityTypeConfiguration<UserByRole>
    {
        public void Configure(EntityTypeBuilder<UserByRole> builder)
        {
            builder.ToTable("UserByRole");
            builder.HasKey(pr => new { pr.IdRole, pr.IdUser });

        }
    }
}
