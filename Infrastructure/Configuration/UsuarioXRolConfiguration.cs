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
    public class UsuarioXRolConfiguration : IEntityTypeConfiguration<UsuarioXRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioXRol> builder)
        {
            builder.ToTable("usuarioXRol");
            builder.HasKey(pr => new { pr.IdRol, pr.IdUsuario });

        }
    }
}
