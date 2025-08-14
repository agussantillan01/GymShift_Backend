using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Enums
{
    public enum ApplicationStatusEnum
    {
        PENDIENTE_APROBACION=1,
        SOLICITUD_APROBADA=2,
        SOLICITUD_DESAPROBADA=3

    }
}
