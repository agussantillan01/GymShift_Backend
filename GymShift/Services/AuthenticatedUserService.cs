using Domain.Interface;
using System.Security.Claims;

namespace GymShift.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
            //IdEmpresa = httpContextAccessor.HttpContext?.User?.FindFirstValue("idEmpresa");
            EsUserSistema = httpContextAccessor.HttpContext?.User?.FindFirstValue("userSistema");
            Nombre = httpContextAccessor.HttpContext?.User?.Identity.Name;
        }

        public string UserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Nombre { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EsUserSistema { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
