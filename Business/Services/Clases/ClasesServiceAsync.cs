using Azure.Core;
using Business.DTOs.Clase;
using Business.Exceptions;
using Domain.Interface;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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

        public async Task<string> Generar(ClaseParemeterDTO Actividad, string user)
        {
            var usuarioLogueado = await _ApplicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.UserName.Trim() == user);
            var validationErrors = new List<string>();
            await Validar(Actividad, validationErrors);

            Evento evento = new Evento();
            evento.IdTipoEvento = Actividad.Actividad;
            evento.FechaInicio = Actividad.FechaInicio;
            evento.FechaFin = Actividad.FechaFin;
            evento.Horario = Actividad.Horario.Trim();
            evento.Duracion = Actividad.Duracion.Trim();
            evento.Dias = string.Join(";", Actividad.Dias);
            evento.IdModalidad = int.Parse(Actividad.Modalidad);
            evento.Valor = Convert.ToDecimal(Actividad.Valor);
            evento.Descripcion = Actividad.Descripcion;
            evento.CupoMaximo = Actividad.CupoMaximo;
            evento.CupoActual = 0;
            evento.IdProfesor = usuarioLogueado.Id;
            await _ApplicationDbContext.AddAsync(evento);
            await _ApplicationDbContext.SaveChangesAsync();

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
