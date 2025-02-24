using Business.DTOs.Account;
using Business.Interfaces;
using Business.Services.Usuarios;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymShift.Controllers.Login
{
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        public AuthController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin));
        }
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAdress()));
        }
        [HttpPost("GetUsuario")]
        public async Task<UsuarioLogin> GetUsuario(AuthenticationRequest request)
        {
            return await _accountService.GetUsuario(request);
        }

        #region FuncionesPrivadas 
        private string GenerateIPAdress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For")) return Request.Headers["X-Forwarded-For"];
            else return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

        }
        #endregion

    }
}
