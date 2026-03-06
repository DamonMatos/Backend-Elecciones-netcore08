using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Masivo
{
    public class UpdateMasivoCuentaCorrienteDTO
    {
        public sealed record UpdateMasivoCuentaCorrienteRequest(
            string codigoNivel,
            string codigoPeriodo,
            string codigoCarrera,
            string fechaInicio,
            int diasPostergacion,
            int cuotaDesde
        );

        public sealed record UpdateMasivoCuentaCorrienteResponse(
            int CodigoEjecucion,
            string Estado,
            string Resultado
        );
    }
}
