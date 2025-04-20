using Business.DTOs.Clase;
using Business.DTOs.Eventos;
using Business.Interfaces;
using Business.Services.Actividades;
using Business.Services.Clases;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymShift.Controllers.Clases
{
    public class ClaseController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private ClasesServiceAsync _ClasesServiceAsync;

        public ClaseController(IAccountService accountService, IConfiguration configuration, ClasesServiceAsync clasesServiceAsync)
        {
            _accountService = accountService;
            _configuration = configuration;
            _ClasesServiceAsync = clasesServiceAsync;
        }

        [HttpPost("GenerarEvento")]
        [Authorize]
        public async Task<IActionResult> GenerarEvento([FromBody] ClaseParemeterDTO clase)
        {
            var user = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _ClasesServiceAsync.Generar(clase, user));
        }
        [HttpGet("GetClasesAprobadasXcoach")]
        [Authorize]
        public async Task<List<MiEventoView>> GetClasesAprobadasXcoach([FromQuery] int id)
        {
            return await _ClasesServiceAsync.ObtenerClasesAprobadasXcoach(id);
        }
        [HttpGet("GetClasesSolicitadasXCoach")]
        [Authorize]
        public async Task<List<MiEventoView>> GetClasesSolicitadasXCoach()
        {
            var user = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return await _ClasesServiceAsync.ObtenerClasesSolicitadasXCoach(user);
        }
        [HttpGet("GetClasesSolicitadas")]
        [Authorize]
        public async Task<List<MiEventoView>> GetClasesSolicitadas()
        {
            return await _ClasesServiceAsync.ObtenerClasesSolicitadas();
        }

    }
}
