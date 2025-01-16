using Business.Interfaces;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.IdentityService
{
    public class RoleService : IRoleService
    {
        public Task CreateRole(string role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRole(string role)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IRoleService>> ObtenerRoles()
        {
            throw new NotImplementedException();
        }

        public Task ValidarRole(int idRole, List<ValidationFailure> validationFailures)
        {
            throw new NotImplementedException();
        }
    }
}
