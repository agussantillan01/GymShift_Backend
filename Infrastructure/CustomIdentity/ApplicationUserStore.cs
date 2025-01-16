using Infrastructure.Contexts;
using Infrastructure.CustomIdentity.Interface;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CustomIdentity
{
    public class ApplicationUserStore : UserStore<UsuarioLogin, Grupo, ApplicationDbContext, int,
        PermisoUsuario, GrupoUsuario, Microsoft.AspNetCore.Identity.IdentityUserLogin<int>, IdentityUserToken<int>, IdentityRoleClaim<int>>, IApplicationUserStore
    {
        public ApplicationUserStore(ApplicationDbContext context)
            : base(context)
        {

        }

        public Definicion ObtenerDominio()
        {
            throw new NotImplementedException();
        }

        public ExcepcionActiveDirectory ObtenerExcepcionActiveDirectory(string usuario)
        {
            throw new NotImplementedException();
        }

        public bool UsaActiveDirectory()
        {
            throw new NotImplementedException();
        }
    }
}
