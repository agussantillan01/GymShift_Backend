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

        public async Task<List<UserView>> ObtenerUsuarios(string userNameLogueado, int pageNumber, int pageSize, string filter)
        {
            var usuarioLogueado = await _ApplicationDbContext.Users
                .FirstOrDefaultAsync(x => x.UserName.Trim() == userNameLogueado);

            if (usuarioLogueado == null)
                return new List<UserView>();

            var query = _ApplicationDbContext.Users
                .Where(x => x.Id != usuarioLogueado.Id);

            if (!usuarioLogueado.IsUserAdmin)
                query = query.Where(x => !x.IsUserAdmin);

            // Aplicar filtro si no está vacío o nulo
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string filterLower = filter.Trim().ToLower();
                query = query.Where(x => x.FirstName.ToLower().Contains(filterLower) ||
                                         x.Email.ToLower().Contains(filterLower));
            }

            var listUsers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var lista = listUsers.Select(item => new UserView()
            {
                Id = item.Id,
                firstName = item.FirstName,
                lastName = item.LastName,
                email = item.Email ?? "No Contiene",
                role = obtenerRol(item.Id)
            }).ToList();
            return lista;
        }


        public async Task<UserEdit> GetUsuario(int idUsuario)
        {
            UserEdit usuarioReturn = new UserEdit();
            var usuario = await _ApplicationDbContext.Users.SingleOrDefaultAsync(x => x.Id == idUsuario);

            usuarioReturn.Id = usuario.Id;
            usuarioReturn.Nombre = usuario.FirstName;
            usuarioReturn.Apellido = usuario.LastName;
            usuarioReturn.Email = usuario.Email == null ? "" : usuario.Email;
            usuarioReturn.UserName = usuario.UserName == null ? "" : usuario.UserName;
            usuarioReturn.Rol = obtenerRol(usuario.Id);
            usuarioReturn.Actividades = usuarioReturn.Rol.ToLower() != "coach" ? new List<string>() : await ObtenerActividades(usuario.Id);
            return usuarioReturn;

        }

        public async Task<Response<string>> Update(UserEdit usuario)
        {

            try
            {
                var user = await _ApplicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == usuario.Id);
                user.FirstName = usuario.Nombre;
                user.LastName = usuario.Apellido;
                user.Email = usuario.Email.Trim();
                user.NormalizedEmail = usuario.Email.ToUpper().Trim();

                await SeteoRolesYProfesiones(usuario.Id, usuario.Rol, usuario.Actividades);
                _ApplicationDbContext.Users.Update(user);
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
            var userXRol = _ApplicationDbContext.UserByRol.FirstOrDefault(x => x.IdUser == id);
            var rol = _ApplicationDbContext.Roles.FirstOrDefault(x => x.Id == userXRol.IdRole);

            return rol.role.ToLower();
        }

        private async Task<List<string>> ObtenerActividades(int id)
        {
            var actXEntrenador = await (_ApplicationDbContext.ActivityByCoach.Where(x => x.IdUser == id)).ToListAsync();
            List<string> actividades = new List<string>();
            foreach (var item in actXEntrenador)
            {
                var objActividad = await _ApplicationDbContext.Activities.FirstOrDefaultAsync(x => x.Id == item.IdActivity);
                actividades.Add(objActividad.activity);
            }
            return actividades;
        }

        private async Task SeteoRolesYProfesiones(int id, string rol, List<string> actividadaes)
        {
            try
            {
                var rolDeUsuarioPrincipal = await _ApplicationDbContext.UserByRol.FirstOrDefaultAsync(x => x.IdUser == id);
                var objRolAntes = await _ApplicationDbContext.Roles.FirstOrDefaultAsync(x => x.Id == rolDeUsuarioPrincipal.IdRole); // objeto rol viejo
                await SeteoRol(id, rol);
                var objRolNuevo = await _ApplicationDbContext.Roles.FirstOrDefaultAsync(x => x.role.ToLower().Trim() == rol); //objeto del nuevo Rol


                if (objRolNuevo.role.ToLower().Trim() == "coach" && objRolAntes.role.ToLower().Trim() != "coach")
                {
                    //Actualizo el usuario, de usuario normal a entrenador
                    await eliminoActividades(id);
                }
                else if (objRolNuevo.role.ToLower().Trim() == "coach" && objRolAntes.role.ToLower().Trim() == "coach")
                {
                    //Modifica usuario de entrenador a tipo entrenador, deberia modificar solamente las actividades
                    await ActualizoActividades(id, actividadaes);
                }
                else if (objRolAntes.role.ToLower().Trim() == "coach" && objRolNuevo.role.ToLower().Trim() != "coach")
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
            var actXus = await (_ApplicationDbContext.ActivityByCoach.Where(x => x.IdUser == idUsuario)).ToListAsync();
            if (actXus.Count > 0)
            {


                _ApplicationDbContext.ActivityByCoach.RemoveRange(actXus);
                await _ApplicationDbContext.SaveChangesAsync();
            }
        }
        private async Task EliminaRol(int idUsuario)
        {
            var userRol = await _ApplicationDbContext.UserByRol.FirstOrDefaultAsync(x => x.IdUser == idUsuario);

            _ApplicationDbContext.UserByRol.Remove(userRol);
            await _ApplicationDbContext.SaveChangesAsync();
        }
        private async Task InsertRoles(int idUsuario, string nombreRol)
        {
            var rol = await _ApplicationDbContext.Roles.FirstOrDefaultAsync(x => x.role.ToLower().Trim() == nombreRol.ToLower().Trim());

            UserRole usXrol = new UserRole();
            usXrol.IdUser = idUsuario;
            usXrol.IdRole = rol.Id;
            await _ApplicationDbContext.UserByRol.AddAsync(usXrol);
            await _ApplicationDbContext.SaveChangesAsync();
        }
        private async Task InsertActividades(int idUsuario, List<string> actividades)
        {
            List<ActivityByCoach> listInsert = new List<ActivityByCoach>();
            foreach (var item in actividades)
            {
                var objActividad = await _ApplicationDbContext.Activities.FirstOrDefaultAsync(x => x.activity.Trim().ToUpper() == item.Trim().ToUpper());
                ActivityByCoach actXEntrenador = new ActivityByCoach();
                actXEntrenador.IdUser = idUsuario;
                actXEntrenador.IdActivity = objActividad.Id;

                listInsert.Add(actXEntrenador);
            }

            await _ApplicationDbContext.ActivityByCoach.AddRangeAsync(listInsert);
            await _ApplicationDbContext.SaveChangesAsync();
        }
        #endregion


    }
}
