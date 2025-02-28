using Business.DTOs.Usuarios;
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
        public async Task<List<UsuarioView>> GetUsuarios(int pageNumber, int pageSize, string filter)
        {
            filter = filter == null ? "" :filter.Trim(); 
            var user= User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return await _UsuarioServiceAsync.ObtenerUsuarios(user, pageNumber, pageSize, filter);
        }
        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Update(UsuarioEdit us)
        {
            return Ok(await _UsuarioServiceAsync.Update(us));
        }




    }
}
