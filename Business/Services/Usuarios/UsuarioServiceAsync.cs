using Business.DTOs.Usuarios;
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

            var lista =  listUsers.Select(item => new UsuarioView()
            {
                Id = item.Id,
                Nombre = item.Nombre,
                Email = item.Email ?? "No Contiene",
                Rol = "Revisar"
            }).ToList();
            return lista;
        }


    }
}
