namespace WsElecciones.Domain.Views.Votante
{
    public class GetVotanteView
    {
        public sealed record ProcesoItem(
            int IdEleccion,
            string NombreEleccion,
            int IdProceso,
            string NombreProceso,
            string Tipodocumento,
            string NumeroDocumento,
            DateTime FechaFin
        );
        public sealed record CandidatoItem(
            int IdEleccion,
            int IdProceso,
            int IdCandidato,
            string NombreCompleto,
            string Area,
            string UrlFile
        );

        public sealed record VotantePageResult(
            IReadOnlyCollection<ProcesoItem> Procesos,
            IReadOnlyCollection<CandidatoItem> Candidatos
        );
    }
}
