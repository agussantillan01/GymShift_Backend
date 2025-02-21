using Business.Interfaces;
using Business.Services.Actividades;
using Business.Services.Usuarios;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymShift.Controllers.Actividades
{
    public class TipoEventoController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private TipoEventoServiceAsync _TipoEventoServiceAsync;

        public TipoEventoController(IAccountService accountService, IConfiguration configuration, TipoEventoServiceAsync tipoEventoServiceAsync)
        {
            _accountService = accountService;
            _configuration = configuration;
            _TipoEventoServiceAsync = tipoEventoServiceAsync;
        }

        [HttpGet("GetTiposEventos")]
        [Authorize]
        public async Task<List<TipoEvento>> GetTiposEventos()
        {
            return await _TipoEventoServiceAsync.GetTiposEventos();
        }
    }
}
