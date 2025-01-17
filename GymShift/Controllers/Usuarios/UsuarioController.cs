using Business.Interfaces;
using Business.Services.Usuarios;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymShift.Controllers.Usuarios
{
    public class UsuarioController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private UsuarioServiceAsync _UsuarioServiceAsync;

        public UsuarioController(IAccountService accountService, IConfiguration configuration, UsuarioServiceAsync usuarioServiceAsync)
        {
            _accountService = accountService;
            _configuration = configuration;
            _UsuarioServiceAsync = usuarioServiceAsync;
        }
        [HttpGet("GetUsuarios")]
        [Authorize]
        public async Task<List<UsuarioLogin>> GetUsuarios()
        {
            return await _UsuarioServiceAsync.Obtener();
        }



    }
}
