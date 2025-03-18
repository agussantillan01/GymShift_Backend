using Business.Interfaces;
using Business.Services.Actividades;
using Business.Services.Modalidades;
using Business.Services.Usuarios;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymShift.Controllers.Modalidades
{
    public class ModalidadController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private ModalidadServiceAsync _ModalidadServiceAsync;

        public ModalidadController(IAccountService accountService, IConfiguration configuration, ModalidadServiceAsync modalidadServiceAsync)
        {
            _accountService = accountService;
            _configuration = configuration;
            _ModalidadServiceAsync = modalidadServiceAsync;
        }

        [HttpGet("GetModalidades")]
        public async Task<List<Modalidad>> GetModalidades()
        {
            return await _ModalidadServiceAsync.ObtenerModalidades();
        }
    }
}
