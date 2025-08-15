using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CustomIdentity.Interface
{
    public interface IApplicationUserStore
    {
        Task AddClaimsAsync(UserLogin user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);
        Definition ObtenerDominio();
        ExcepcionActiveDirectory ObtenerExcepcionActiveDirectory(string usuario);
        bool UsaActiveDirectory();
    }
}
