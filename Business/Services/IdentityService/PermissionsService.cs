using Business.DTOs.Account;
using Business.Interfaces;
using Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using IPermission = Infrastructure.CustomIdentity.Interface.IPermission;
using System.Threading.Tasks;

namespace Business.Services.IdentityService
{
    public class PermissionsService : IPermissionsService
    {
        public Task<List<string>> GetFieldPermissionsAsync(string form)
        {
            throw new NotImplementedException();
        }

        public Task<List<IPermission>> GetPermissionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetScreenPermissionsAsync(string value)
        {
            throw new NotImplementedException();
        }

        public Task<Response<PermissionDTO>> ObtenerPermisosRoles(string role)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRolePermission(PermissionDTO model)
        {
            throw new NotImplementedException();
        }

        public bool ValidateMenuPermissions(List<IPermission> permissions, string claimValue)
        {
            throw new NotImplementedException();
        }
    }
}
