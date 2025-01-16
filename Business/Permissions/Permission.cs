using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Permissions
{
    public class Permission
    {
        public const string PREFIJO_PERMISOS_MENU = "Permissions.Menu.";
        public const string CLAIMTYPE_PERMISOS = "Permission";

        public static List<Type> GetAllTypes()
        {
            return new List<Type>
            {
                //typeof (Reporte)
            };
        }
    }
}
