using Azure.Core;
using Business.DTOs.Clase;
using Business.Exceptions;
using Domain.Interface;
using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Clases
{
    public class ClasesServiceAsync
    {
        #region atributos 
        private readonly IConexion _conexion;
        private readonly ApplicationDbContext _ApplicationDbContext;
        #endregion

        public ClasesServiceAsync(ApplicationDbContext ApplicationDbContext,
                                IConexion conexion
                                )
        {
            _conexion = conexion;
            _ApplicationDbContext = ApplicationDbContext;
        }

        public async Task<string> Generar(ClaseParemeterDTO Actividad)
        {
            var validationErrors = new List<string>();
            await Validar(Actividad, validationErrors);

            
            return "";

        }
        public async Task Validar(ClaseParemeterDTO Actividad, List<string> validations)
        {
            if (Actividad.Dias == null || Actividad.Dias.Count == 0)
            {
                validations.Add("Debe ingresar al menos un día.");
            }
            if (Actividad.FechaInicio == null)
            {
                validations.Add("Debe ingresar una fecha de Inicio.");
            }
            if (Actividad.FechaFin == null)
            {
                validations.Add("Debe ingresar una fecha de Fin.");
            }
            if (Actividad.CupoMaximo == null || Actividad.CupoMaximo == 0)
            {
                validations.Add("Debe ingresar un cupo maximo.");
            }
            if (Actividad.Duracion == null || int.Parse(Actividad.Duracion) == 0)
            {
                validations.Add("Debe ingresar una Duracion.");
            }
            if(Actividad.FechaInicio > Actividad.FechaFin)
            {
                validations.Add("La fecha de inicio debe ser mayor a la fecha de hoy");
            }

            if (validations.Count > 0)
            {
                throw new ValidationException(validations);
            }
        }
    }
}
