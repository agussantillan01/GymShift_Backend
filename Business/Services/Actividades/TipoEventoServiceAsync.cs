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

        public async Task<List<TypeOfClass>> GetTiposEventos()
        {
            return await _ApplicationDbContext.TypesClasses.ToListAsync();
        }
        public async Task<List<TypeOfClass>> ObtenerDeportesXcoach(string usernameLogueado)
        {
            var usuarioLogueado = await _ApplicationDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Trim() == usernameLogueado);

            if (usuarioLogueado == null)
            {
                return new List<TypeOfClass>();
            }

            var deportesXusuario = await _ApplicationDbContext.ActivityByCoach
                .Where(ae => ae.IdUser == usuarioLogueado.Id) 
                .Join(
                    _ApplicationDbContext.TypesClasses,  
                    ae => ae.IdActivity,             
                    td => td.Id,                       
                    (ae, td) => new TypeOfClass         
                    {
                        Id = td.Id,
                        Type = td.Type
                    }
                )
                .Distinct()
                .ToListAsync();

            return deportesXusuario;
        }
    }
}
