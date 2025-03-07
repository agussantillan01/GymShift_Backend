using Business.DTOs.Usuarios;
using Domain.Entities;
using Domain.Interface;
using Domain.Settings;
using Infrastructure.Contexts;
using Infrastructure.CustomIdentity.Interface;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Wrappers;
using Azure.Core;
using Business.Exceptions;


namespace Business.Services.Usuarios
{
    public class UsuarioServiceAsync
    {
        #region atributos 
        private readonly IConexion _conexion;
        private readonly ApplicationDbContext _ApplicationDbContext;
        #endregion

        public UsuarioServiceAsync(ApplicationDbContext ApplicationDbContext,
                                IConexion conexion
                                )
        {
            _conexion = conexion;
            _ApplicationDbContext = ApplicationDbContext;
        }

        public async Task<List<UsuarioView>> ObtenerUsuarios(string userNameLogueado, int pageNumber, int pageSize, string filter)
        {
            var usuarioLogueado = await _ApplicationDbContext.Usuarios
                .FirstOrDefaultAsync(x => x.UserName.Trim() == userNameLogueado);

            if (usuarioLogueado == null)
                return new List<UsuarioView>();

            var query = _ApplicationDbContext.Usuarios
                .Where(x => x.Id != usuarioLogueado.Id);

            if (!usuarioLogueado.EsUserSistema)
                query = query.Where(x => !x.EsUserSistema);

            // Aplicar filtro si no está vacío o nulo
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string filterLower = filter.Trim().ToLower();
                query = query.Where(x => x.Nombre.ToLower().Contains(filterLower) ||
                                         x.Email.ToLower().Contains(filterLower));
            }

            var listUsers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var lista = listUsers.Select(item => new UsuarioView()
            {
                Id = item.Id,
                Nombre = item.Nombre,
                Apellido = item.Apellido,
                Email = item.Email ?? "No Contiene",
                Rol = obtenerRol(item.Id)
            }).ToList();
            return lista;
        }


        public async Task<UsuarioEdit> GetUsuario(int idUsuario)
        {
            UsuarioEdit usuarioReturn = new UsuarioEdit();
            var usuario = await _ApplicationDbContext.Usuarios.SingleOrDefaultAsync(x => x.Id == idUsuario);

            usuarioReturn.Id = usuario.Id;
            usuarioReturn.Nombre = usuario.Nombre;
            usuarioReturn.Apellido = usuario.Apellido;
            usuarioReturn.Email = usuario.Email == null ? "" : usuario.Email;
            usuarioReturn.UserName = usuario.UserName == null ? "" : usuario.UserName;
            usuarioReturn.Rol = obtenerRol(usuario.Id);
            usuarioReturn.Actividades = usuarioReturn.Rol.ToLower() != "coach" ? new List<string>() : await ObtenerActividades(usuario.Id);
            return usuarioReturn;

        }

        public async Task<Response<string>> Update(UsuarioEdit usuario)
        {

            try
            {
                var user = await _ApplicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == usuario.Id);
                user.Nombre = usuario.Nombre;
                user.Apellido = usuario.Apellido;
                user.Email = usuario.Email.Trim();
                user.NormalizedEmail = usuario.Email.ToUpper().Trim();

                await SeteoRolesYProfesiones(usuario.Id, usuario.Rol, usuario.Actividades);
                _ApplicationDbContext.Usuarios.Update(user);
                await _ApplicationDbContext.SaveChangesAsync();
                return new Response<string>(usuario.Id.ToString(), message: $"Usuario Modificado.");
            }
            catch (Exception ex)
            {

                throw new ApiException($"Ocurrió un error al modificar el usuario");
            }

        }

        #region funcionesPrivadas 

        private string obtenerRol(int id)
        {
            var userXRol = _ApplicationDbContext.UsuarioXRol.FirstOrDefault(x => x.IdUsuario == id);
            var rol = _ApplicationDbContext.Roles.FirstOrDefault(x => x.Id == userXRol.IdRol);

            return rol.Nombre.ToLower();
        }

        private async Task<List<string>> ObtenerActividades(int id)
        {
            var actXEntrenador = await (_ApplicationDbContext.ActividadesXEntrenador.Where(x => x.IdUsuario == id)).ToListAsync();
            List<string> actividades = new List<string>();
            foreach (var item in actXEntrenador)
            {
                var objActividad = await _ApplicationDbContext.TiposDeEventos.FirstOrDefaultAsync(x => x.Id == item.IdActividad);
                actividades.Add(objActividad.Nombre);
            }
            return actividades;
        }

        private async Task SeteoRolesYProfesiones(int id, string rol, List<string> actividadaes)
        {
            try
            {
                var rolDeUsuarioPrincipal = await _ApplicationDbContext.UsuarioXRol.FirstOrDefaultAsync(x => x.IdUsuario == id);
                var objRolAntes = await _ApplicationDbContext.Roles.FirstOrDefaultAsync(x => x.Id == rolDeUsuarioPrincipal.IdRol); // objeto rol viejo
                await SeteoRol(id, rol);
                var objRolNuevo = await _ApplicationDbContext.Roles.FirstOrDefaultAsync(x => x.Nombre.ToLower().Trim() == rol); //objeto del nuevo Rol


                if (objRolNuevo.Nombre.ToLower().Trim() == "coach" && objRolAntes.Nombre.ToLower().Trim() != "coach")
                {
                    //Actualizo el usuario, de usuario normal a entrenador
                    await eliminoActividades(id);
                }
                else if (objRolNuevo.Nombre.ToLower().Trim() == "coach" && objRolAntes.Nombre.ToLower().Trim() == "coach")
                {
                    //Modifica usuario de entrenador a tipo entrenador, deberia modificar solamente las actividades
                    await ActualizoActividades(id, actividadaes);
                }
                else if (objRolAntes.Nombre.ToLower().Trim() == "coach" && objRolNuevo.Nombre.ToLower().Trim() != "coach")
                {
                    //Modifica usuario entrenador a alumno
                    await InsertActividades(id, actividadaes);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        private async Task SeteoRol(int id, string rol)
        {
            string nombreRol = obtenerRol(id);
            if (nombreRol == rol.ToLower().Trim())
            {
                return;
            }
            else
            {
                //elimino roles 
                await EliminaRol(id);
                //InsertRoles
                await InsertRoles(id, rol);
            }
        }
        private async Task ActualizoActividades(int idUsuario, List<string> actividades)
        {
            //elimino actividades
            await eliminoActividades(idUsuario);
            //Inserta actividades 
            await InsertActividades(idUsuario, actividades);
        }
        private async Task eliminoActividades(int idUsuario)
        {
            var actXus = await (_ApplicationDbContext.ActividadesXEntrenador.Where(x => x.IdUsuario == idUsuario)).ToListAsync();
            if (actXus.Count > 0)
            {


                _ApplicationDbContext.ActividadesXEntrenador.RemoveRange(actXus);
                await _ApplicationDbContext.SaveChangesAsync();
            }
        }
        private async Task EliminaRol(int idUsuario)
        {
            var userRol = await _ApplicationDbContext.UsuarioXRol.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);

            _ApplicationDbContext.UsuarioXRol.Remove(userRol);
            await _ApplicationDbContext.SaveChangesAsync();
        }
        private async Task InsertRoles(int idUsuario, string nombreRol)
        {
            var rol = await _ApplicationDbContext.Roles.FirstOrDefaultAsync(x => x.Nombre.ToLower().Trim() == nombreRol.ToLower().Trim());

            UsuarioXRol usXrol = new UsuarioXRol();
            usXrol.IdUsuario = idUsuario;
            usXrol.IdRol = rol.Id;
            await _ApplicationDbContext.UsuarioXRol.AddAsync(usXrol);
            await _ApplicationDbContext.SaveChangesAsync();
        }
        private async Task InsertActividades(int idUsuario, List<string> actividades)
        {
            List<ActividadesXEntrenador> listInsert = new List<ActividadesXEntrenador>();
            foreach (var item in actividades)
            {
                var objActividad = await _ApplicationDbContext.TiposDeEventos.FirstOrDefaultAsync(x => x.Nombre.Trim().ToUpper() == item.Trim().ToUpper());
                ActividadesXEntrenador actXEntrenador = new ActividadesXEntrenador();
                actXEntrenador.IdUsuario = idUsuario;
                actXEntrenador.IdActividad = objActividad.Id;

                listInsert.Add(actXEntrenador);
            }

            await _ApplicationDbContext.ActividadesXEntrenador.AddRangeAsync(listInsert);
            await _ApplicationDbContext.SaveChangesAsync();
        }
        #endregion


    }
}
