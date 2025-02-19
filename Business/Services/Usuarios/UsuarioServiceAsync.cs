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

        public async Task<List<UsuarioLogin>> ObtenerUsuarios(string userNameLogueado)
        {
            var usuarioLogueado = await _ApplicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.UserName.Trim() == userNameLogueado);
            List<UsuarioLogin> listUsers = new List<UsuarioLogin>();
            switch (usuarioLogueado.EsUserSistema)
            {
                case true:
                    listUsers = await _ApplicationDbContext.Usuarios.Where(x => x.Id != usuarioLogueado.Id).ToListAsync();
                    break;
                default:
                    listUsers = await _ApplicationDbContext.Usuarios.Where(x => x.Id != usuarioLogueado.Id && !x.EsUserSistema).ToListAsync();
                break;
            }

            return listUsers;
        }
    }
}
