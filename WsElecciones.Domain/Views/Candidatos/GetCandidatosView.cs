using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.Candidatos
{
    public class GetCandidatosView
    {
        public sealed record CandidatoItem(
            int IdEleccion,
            int IdProceso,
            int IdCandidato,
            string TipoDocumento,
            string NumeroDocumento,
            string NombreCompleto,
            string Area,
            string Localidad,
            string UrlFile,
            int Estado,
            string Descripcion
            );

        public sealed record CandidatoPagedResult(
            IReadOnlyCollection<CandidatoItem> Items,
            int TotalRegistros,
            int Page,
            int Limit
            );

    }
}
