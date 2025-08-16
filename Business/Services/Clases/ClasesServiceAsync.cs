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
using Business.Enums;
using Business.DTOs.Class;

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

        public async Task<string> Insert(ClassParemeterDTO Actividad, string user)
        {
            try
            {
                var usuarioLogueado = await _ApplicationDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Trim() == user);
                if (usuarioLogueado == null) throw new ApiException($"Ocurrió con tus creedenciales. Por favor Comuníquese.");
                var validationErrors = new List<string>();
                await Validar(Actividad, validationErrors);

                Class evento = new Class();
                evento.idActivity = Actividad.Actividad;
                evento.DateFrom = Actividad.FechaInicio;
                evento.DateTo = Actividad.FechaFin;
                evento.Schedule = Actividad.Horario.Trim();
                evento.Duration = Actividad.Duracion.Trim();
                evento.Days = string.Join(";", Actividad.Dias);
                evento.IdModality = int.Parse(Actividad.Modalidad);
                evento.Price = Convert.ToDecimal(Actividad.Valor);
                evento.Description = Actividad.Descripcion;
                evento.AmountMax = Actividad.CupoMaximo;
                evento.Amount = 0;
                evento.IdCoach = usuarioLogueado.Id;
                evento.ApplicationStatus = (int)ApplicationStatusEnum.PENDIENTE_APROBACION;

                await _ApplicationDbContext.AddAsync(evento);
                await _ApplicationDbContext.SaveChangesAsync();

                return "";
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<string> AprobarClase(int idClase) 
        {
            try
            {
                var evento = await _ApplicationDbContext.Classes.FirstOrDefaultAsync(x => x.Id == idClase);
                evento.ApplicationStatus = (int)ApplicationStatusEnum.SOLICITUD_APROBADA;

                _ApplicationDbContext.Update(evento);
                await _ApplicationDbContext.SaveChangesAsync();
                return "";

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<string> Update(ClassParemeterDTO Actividad)
        {
            try
            {
                var evento = await _ApplicationDbContext.Classes.FirstOrDefaultAsync(x => x.Id == Actividad.Id);
                evento.idActivity = Actividad.Actividad;
                evento.DateFrom = Actividad.FechaInicio;
                evento.DateTo = Actividad.FechaFin;
                evento.Schedule = Actividad.Horario.Trim();
                evento.Duration = Actividad.Duracion.Trim();
                evento.Days = string.Join(";", Actividad.Dias);
                evento.IdModality = int.Parse(Actividad.Modalidad);
                evento.Price = Convert.ToDecimal(Actividad.Valor);
                evento.Description = Actividad.Descripcion;
                evento.AmountMax = Actividad.CupoMaximo;
                evento.Amount = 0;

                _ApplicationDbContext.Update(evento);
                await _ApplicationDbContext.SaveChangesAsync();

                return $"Clase actualizada #${evento.Id} correctamente";
            }
            catch (Exception)
            {

                throw;
            }



        }
        public async Task Validar(ClassParemeterDTO Actividad, List<string> validations)
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
        public async Task<List<MyClassView>> ObtenerClasesAprobadasXcoach(int IdCoach)
        {
            var eventos = await CargarListaMisEventos(IdCoach, (int)ApplicationStatusEnum.SOLICITUD_APROBADA);

            return eventos;
        }
        public async Task<List<MyClassView>> CargarListaMisEventos(int idCoach, int estado)
        {
            try
            {
                List<MyClassView> eventos = new List<MyClassView>();
                var listaBase = await _ApplicationDbContext.Classes.Where(x => x.IdCoach == idCoach && x.ApplicationStatus == estado).ToListAsync();
                foreach (Class item in listaBase)
                {
                    MyClassView evt = new MyClassView();
                    evt.Id = item.Id;
                    evt.TipoEvento = await ObtenerNombreActividad(item.idActivity);
                    evt.FechaInicio = item.DateFrom;
                    evt.FechaFin = item.DateTo;
                    evt.Horario = item.Schedule;
                    evt.Duracion = item.Duration;
                    evt.Dias = item.Days;
                    evt.Modalidad = await ObtenerNombreModalidad(item.IdModality);
                    evt.Valor = item.Price;
                    evt.Descripcion = item.Description;
                    evt.CupoMaximo = item.AmountMax;
                    evt.CupoActual = item.Amount;
                    evt.Profesor = await ObtenerNombreApeXCoach(item.IdCoach);
                    eventos.Add(evt);
                }
                return eventos;
            }
            catch (Exception ex)
            {

                throw ex;
            }

           
        }



        public async Task<List<MyClassView>> ObtenerClasesSolicitadasXCoach(string user)
        {
            var usuarioLogueado = await _ApplicationDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Trim() == user);
            var eventos = await CargarListaMisEventos(usuarioLogueado.Id, (int)ApplicationStatusEnum.PENDIENTE_APROBACION);

            return eventos;
        }

        public async Task<List<MyClassView>> ObtenerClasesSolicitadas()
        {
            List<MyClassView> eventos = new List<MyClassView>();
            var listaBase = await _ApplicationDbContext.Classes.Where(x => x.ApplicationStatus == (int)ApplicationStatusEnum.PENDIENTE_APROBACION).ToListAsync();
            foreach (Class item in listaBase)
            {
                MyClassView evt = new MyClassView();
                evt.Id = item.Id;
                evt.TipoEvento = await ObtenerNombreActividad(item.idActivity);
                evt.FechaInicio = item.DateFrom;
                evt.FechaFin = item.DateTo;
                evt.Horario = item.Schedule;
                evt.Duracion = item.Duration;
                evt.Dias = item.Days;
                evt.Modalidad = await ObtenerNombreModalidad(item.IdModality);
                evt.Valor = item.Price;
                evt.Descripcion = item.Description;
                evt.CupoMaximo = item.AmountMax;
                evt.CupoActual = item.Amount;
                evt.Profesor = await ObtenerNombreApeXCoach(item.IdCoach);
                eventos.Add(evt);
            }
            return eventos;
        }
        private async Task<string> ObtenerNombreApeXCoach(int id)
        {
            var usuario = await _ApplicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return $"{usuario.FirstName} {usuario.LastName}";
        }
        private async Task<string> ObtenerNombreModalidad(int idModalidad)
        {
            return (await _ApplicationDbContext.Modalities.FirstOrDefaultAsync(x=> x.Id== idModalidad)).modality;
        }
        private async Task<string> ObtenerNombreActividad(int idActividad)
        {
            return (await _ApplicationDbContext.Activities.FirstOrDefaultAsync(x => x.Id == idActividad)).activity;
        }
    }
}
