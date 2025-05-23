﻿using Azure.Core;
using Business.DTOs.Clase;
using Business.DTOs.Eventos;
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
            try
            {
                var usuarioLogueado = await _ApplicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.UserName.Trim() == user);
                if (usuarioLogueado == null) throw new ApiException($"Ocurrió con tus creedenciales. Por favor Comuníquese.");
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
                evento.EstadoSolicitud = "PENDIENTE_APROBACION";
                await _ApplicationDbContext.AddAsync(evento);
                await _ApplicationDbContext.SaveChangesAsync();

                return "";
            }
            catch (Exception ex)
            {

                throw;
            }


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
            if (Actividad.FechaInicio > Actividad.FechaFin)
            {
                validations.Add("La fecha de inicio debe ser mayor a la fecha de hoy");
            }

            if (validations.Count > 0)
            {
                throw new ValidationException(validations);
            }
        }
        public async Task<List<MiEventoView>> ObtenerClasesAprobadasXcoach(int IdCoach)
        {
            var eventos = await CargarListaMisEventos(IdCoach, "APROBADO");

            return eventos;
        }
        public async Task<List<MiEventoView>> CargarListaMisEventos(int idCoach, string estado)
        {
            List<MiEventoView> eventos = new List<MiEventoView>();
            var listaBase = await _ApplicationDbContext.Eventos.Where(x => x.IdProfesor == idCoach && x.EstadoSolicitud== estado).ToListAsync();
            foreach (Evento item in listaBase)
            {
                MiEventoView evt = new MiEventoView();
                evt.Id = item.Id;
                evt.TipoEvento = await ObtenerNombreActividad(item.IdTipoEvento);
                evt.FechaInicio = item.FechaInicio;
                evt.FechaFin = item.FechaFin;
                evt.Horario = item.Horario;
                evt.Duracion = item.Duracion;
                evt.Dias = item.Dias;
                evt.Modalidad = await ObtenerNombreModalidad(item.IdModalidad);
                evt.Valor = item.Valor;
                evt.Descripcion = item.Descripcion;
                evt.CupoMaximo = item.CupoMaximo;
                evt.CupoActual = item.CupoActual;
                evt.Profesor = await ObtenerNombreApeXCoach(item.IdProfesor);
                eventos.Add(evt);
            }
            return eventos;
        }



        public async Task<List<MiEventoView>> ObtenerClasesSolicitadasXCoach(string user)
        {
            var usuarioLogueado = await _ApplicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.UserName.Trim() == user);
            var eventos = await CargarListaMisEventos(usuarioLogueado.Id, "PENDIENTE_APROBACION");

            return eventos;
        }

        public async Task<List<MiEventoView>> ObtenerClasesSolicitadas()
        {
            List<MiEventoView> eventos = new List<MiEventoView>();
            var listaBase = await _ApplicationDbContext.Eventos.Where(x => x.EstadoSolicitud.Trim().ToUpper() == "PENDIENTE_APROBACION").ToListAsync();
            foreach (Evento item in listaBase)
            {
                MiEventoView evt = new MiEventoView();
                evt.Id = item.Id;
                evt.TipoEvento = await ObtenerNombreActividad(item.IdTipoEvento);
                evt.FechaInicio = item.FechaInicio;
                evt.FechaFin = item.FechaFin;
                evt.Horario = item.Horario;
                evt.Duracion = item.Duracion;
                evt.Dias = item.Dias;
                evt.Modalidad = await ObtenerNombreModalidad(item.IdModalidad);
                evt.Valor = item.Valor;
                evt.Descripcion = item.Descripcion;
                evt.CupoMaximo = item.CupoMaximo;
                evt.CupoActual = item.CupoActual;
                evt.Profesor = await ObtenerNombreApeXCoach(item.IdProfesor);
                eventos.Add(evt);
            }
            return eventos;
        }
        private async Task<string> ObtenerNombreApeXCoach(int id)
        {
            var usuario = await _ApplicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            return $"{usuario.Nombre} {usuario.Apellido}";
        }
        private async Task<string> ObtenerNombreModalidad(int idModalidad)
        {
            return (await _ApplicationDbContext.Modalidades.FirstOrDefaultAsync(x=> x.Id== idModalidad)).modalidad;
        }
        private async Task<string> ObtenerNombreActividad(int idActividad)
        {
            return (await _ApplicationDbContext.TiposDeEventos.FirstOrDefaultAsync(x => x.Id == idActividad)).Nombre;
        }
    }
}
