using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WsElecciones.Application.DTOs.Candidatos.GetCandidatosDTO;

namespace WsElecciones.Application.DTOs.Colaboradores
{
    public class GetColaboradoresDTO
    {
        public sealed record ColaboradorItemDTO(
            int IdEleccion,
            int IdProceso,
            string TipoDocumento,
            string NumeroDocumento,
            string Cargo,
            string Sede,
            string Nombre,
            string ApellidoPaterno,
            string ApellidoMaterno,
            DateTime? FechaRegistro,
            string EmailDifusion,
            int IdUsuario,
            int Estado
        );

        public sealed record ColaboradoresPagedResponse(
            IReadOnlyCollection<ColaboradorItemDTO> Items,
            int TotalRegistros,
            int Page,
            int Limit
            );
    }
}
