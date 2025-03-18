using Business.Services.Actividades;
using Domain.Interface;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Modalidades
{
    public class ModalidadServiceAsync
    {
        #region atributos 
        private readonly IConexion _conexion;
        private readonly ApplicationDbContext _ApplicationDbContext;
        #endregion

        public ModalidadServiceAsync(ApplicationDbContext ApplicationDbContext,
                                IConexion conexion
                                )
        {
            _conexion = conexion;
            _ApplicationDbContext = ApplicationDbContext;
        }


        public async Task<List<Modalidad>> ObtenerModalidades()
        {
            return await _ApplicationDbContext.Modalidades.ToListAsync();
        }

    }
}
