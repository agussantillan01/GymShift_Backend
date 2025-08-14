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
    public enum EstadoSolicitudEnum
    {
        PENDIENTE_APROBACION,
        SOLICITUD_APROBADA,
        SOLICITUD_DESAPROBADA

    }
}
