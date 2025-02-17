using Business.DTOs.Account;
using Business.Exceptions;
using Business.Interfaces;
using Domain.Entities;
using Domain.EntitiesIdentity;
using Domain.Settings;
using Domain.Wrappers;
using Infrastructure.Contexts;
using Infrastructure.CustomIdentity.Interface;
using Infrastructure.Helpers;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly SignInManager<UsuarioLogin> _SingInManager;
        #endregion

        #region Constructor
        public AccountService(ApplicationDbContext ApplicationDbContext,
                                IActiveDirectoryManager activeDirectoryManager,
                                UserManager<UsuarioLogin> userManager,
                                IOptions<JWTSettings> jwtSetting,
                                SignInManager<UsuarioLogin> SingInManager
                                )
        {
            _activeDirectoryManager = activeDirectoryManager;
            _ApplicationDbContext = ApplicationDbContext;
            _userManager = userManager;
            _jwtSetting = jwtSetting.Value;
            _SingInManager = SingInManager;
        }
        #endregion 
        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var users = await _ApplicationDbContext.Usuarios.ToListAsync();
            var user = await GetUsuario(request);
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress, user.Id);
            await UpdateRefreshToken(refreshToken);

            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id.ToString();
            response.Email = user.Email;
            response.IsVerified = true;
            response.RefreshToken = refreshToken.Token;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.ExpireDate = jwtSecurityToken.ValidTo.ToLocalTime();
            response.usuario = await GetUsuarioXId(user.Id);
            return new Response<AuthenticationResponse>(response, $"{user.Email.Trim()} autenticado");
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

        public async Task<UsuarioLogin> GetUsuario(AuthenticationRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Usuario);
            if (user == null)
            {
                throw new ApiException("Email y/o contraseña incorrecta");
            }
            await validarLogin(request, user);
            return user;
        }

        public async Task<Usuario> GetUsuarioXId(int id)
        {
            var user = await _ApplicationDbContext.Usuarios.FirstOrDefaultAsync(x=> x.Id == id);
            Usuario us = new Usuario();
            us.Id = id;
            us.Apellido = user.Apellido;
            us.Nombre = user.Nombre;
            if (user.EsUserSistema)
            {
                us.Role = ["ADMIN"];
                us.Permissions = await getAllPermissions();
            }
            else {
                us.Role = await GetRolesXUsuario(id);
                us.Permissions = await getPermissionsXRole(us.Role);
            }
            return  us;

        }

        private async Task<string[]> getAllPermissions()
        {
            var permissionsDB = await _ApplicationDbContext.Permisos.ToListAsync();
            return permissionsDB.Select(p => p.ClaimType).ToArray();
        }
        private async Task<string[]> getPermissionsXRole(string[] rolesTypes)
        {
            var roles = await _ApplicationDbContext.Roles
                .Where(r => rolesTypes.Contains(r.Nombre.Trim()))
                .ToListAsync();

            var roleIds = roles.Select(r => r.Id).ToList();

            var permissionsDB = await _ApplicationDbContext.PermisoXRol
                .Where(pxr => roleIds.Contains(pxr.IdRol))
                .Select(pxr => pxr.Permiso.ClaimType) 
                .Distinct() 
                .ToArrayAsync(); 

            return permissionsDB;
        }
        private async Task<string[]> GetRolesXUsuario(int id)
        {
            var IdsRoles = await _ApplicationDbContext.UsuarioXRol
                .Where(x => x.IdUsuario == id)
                .ToListAsync();

            var roles = new List<string>();

            foreach (var item in IdsRoles)
            {
                var rol = await _ApplicationDbContext.Roles
                    .FirstOrDefaultAsync(x => x.Id == item.IdRol);

                if (rol != null)
                {
                    roles.Add(rol.Nombre); 
                }
            }

            return roles.ToArray();
        }

        public Task<Response<AuthenticationResponse>> RefreshToken(int userId, string refreshToken, string userName, string idEmpresa, string ip)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"El usuario '{request.UserName}' ya se encuentra tomado.");
            }
            userWithSameUserName = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"El mail '{request.Email}' ya se encuentra tomado.");
            }
            

            var user = new UsuarioLogin
            {
                Email = request.Email,
                Nombre = request.FirstName,
                Apellido = request.LastName,
                UserName = request.UserName,
                NormalizedEmail = request.Email.ToUpper(),
                NormalizedUserName = request.UserName.ToUpper()
            };

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
                return new Response<string>(user.Id.ToString(), message: $"Usuario registrado.");
            else
                throw new ApiException($"{result.Errors}");
        }

        public Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            throw new NotImplementedException();
        }

        #region metodosPrivados 
        private async Task<JwtSecurityToken> GenerateJWToken(UsuarioLogin user)
        {
            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName.Trim()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, user.Email.Trim()),
                 new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName.Trim()),
                 new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.UserName.Trim()),
                 new Claim("uid", user.Id.ToString()),
                 new Claim("userSistema", user.EsUserSistema.ToString()),
                 new Claim("ip", ipAddress),
                 //new Claim("idEmpresa", idEmpresa.Trim())
            };

            //var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key.PadRight(256)));
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSetting.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        private async Task validarLogin(AuthenticationRequest request, UsuarioLogin usuario)
        {
            if (!_activeDirectoryManager.UsaActiveDirectory(usuario.NormalizedUserName))
            {
                var result = await _SingInManager.CheckPasswordSignInAsync(usuario, request.Password.Trim(), lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    throw new ApiException("Usuario y/o contraseña incorrecta.");
                }

            }
            else
            {
                _activeDirectoryManager.Login(usuario.Email, request.Password);
            }
        }
        private RefreshToken GenerateRefreshToken(string ipAdress, int user)
        {
            return new RefreshToken
            {
                Token = RandomTokenStrin(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UsuarioId = user,
                CreatedByIp = ipAdress
            };
        }

        private string RandomTokenStrin()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task UpdateRefreshToken(RefreshToken refreshToken, string ultimoToken = "")
        {
            //var vencidos = await _ApplicationDbContext.RefreshTokens = 
        }
        #endregion
    }
}
