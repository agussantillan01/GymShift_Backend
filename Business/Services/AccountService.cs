using Business.DTOs.Account;
using Business.Interfaces;
using Domain.Settings;
using Domain.Wrappers;
using Infrastructure.Contexts;
using Infrastructure.CustomIdentity.Interface;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AccountService : IAccountService
    {
        #region atributos 
        private readonly IActiveDirectoryManager _activeDirectoryManager;
        private readonly JWTSettings _jwtSetting;
        private readonly UserManager<UsuarioLogin> _userManager;
        private readonly ApplicationDbContext _ApplicationDbContext;
        //private readonly SignInManager<UsuarioLogin> _SingInManager;
        #endregion

        #region Constructor
        public AccountService(ApplicationDbContext ApplicationDbContext,
                                IActiveDirectoryManager activeDirectoryManager,
                                UserManager<UsuarioLogin> userManager,
                                IOptions<JWTSettings> jwtSetting
                                //SignInManager<UsuarioLogin> SingInManager
                                )
        {
            _activeDirectoryManager = activeDirectoryManager;
            _ApplicationDbContext = ApplicationDbContext;
            _userManager = userManager;
            _jwtSetting = jwtSetting.Value;
            //_SingInManager = SingInManager;
        }
        #endregion 
        public Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            throw new NotImplementedException();
        }

        public Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioLogin> GetUsuario(AuthenticationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioLogin> GetUsuarioXId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<AuthenticationResponse>> RefreshToken(int userId, string refreshToken, string userName, string idEmpresa, string ip)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
