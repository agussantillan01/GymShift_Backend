using Domain.Interface;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Actividades
{
    public class TipoEventoServiceAsync
    {
        #region atributos 
        private readonly IConexion _conexion;
        private readonly ApplicationDbContext _ApplicationDbContext;
        #endregion

        public TipoEventoServiceAsync(ApplicationDbContext ApplicationDbContext,
                                IConexion conexion
                                )
        {
            _conexion = conexion;
            _ApplicationDbContext = ApplicationDbContext;
        }

        public async Task<List<TipoEvento>> GetTiposEventos()
        {
            return await _ApplicationDbContext.TiposDeEventos.ToListAsync();
        }
        public async Task<List<TipoEvento>> ObtenerDeportesXcoach(string usernameLogueado)
        {
            var usuarioLogueado = await _ApplicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.UserName.Trim() == usernameLogueado);

            if (usuarioLogueado == null)
            {
                return new List<TipoEvento>();
            }

            var deportesXusuario = await _ApplicationDbContext.ActividadesXEntrenador
                .Where(ae => ae.IdUsuario == usuarioLogueado.Id) 
                .Join(
                    _ApplicationDbContext.TiposDeEventos,  
                    ae => ae.IdActividad,             
                    td => td.Id,                       
                    (ae, td) => new TipoEvento         
                    {
                        Id = td.Id,
                        Nombre = td.Nombre
                    }
                )
                .Distinct()
                .ToListAsync();

            return deportesXusuario;
        }
    }
}
