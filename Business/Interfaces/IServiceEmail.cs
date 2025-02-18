using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IServiceEmail
    {
        Task EnvioMail(string emailReceptor, string constAsunto, string clave, string nombre);
    }
}
