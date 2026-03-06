using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Masivo
{
    public class GetMasivoCuentaCorrienteDTO
    {
        public sealed record GetMasivoCuentaCorrienteRequest(
            int Page = 1,
            int Limit = 10,
            string? Estado = null,
            int? CodigoEjecucion = null
        );

        public sealed record GetMasivoCuentaCorrienteItemDTO(
           string codAlumno,
           string nombreAlumno,
           string codCampus,
           string nombreCampus,
           string codFacultad,
           string codCarrera,
           string nombreCarrera,
           string codDepartamento,
           string nombreDepartamento,
           string estado,
           string nombreIncidente
       );
        public sealed record GetMasivoCuentaCorrienteResponse(
            IReadOnlyCollection<GetMasivoCuentaCorrienteItemDTO> Items,
            int TotalRegistros,
            int Page,
            int Limit);
    }
}
