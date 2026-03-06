using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views
{
    public sealed record ActualizacionMasivo(
        string codCompania,
        string codigoNivel,
        string codigoPeriodo,
        string codigoCarrera,
        string fechaInicio,
        int diasPostergacion,
        int cuotaDesde,
        string Usuario
     );    

}
