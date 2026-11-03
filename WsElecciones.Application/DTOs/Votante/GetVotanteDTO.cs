namespace WsElecciones.Application.DTOs.Votante
{
    public class GetVotanteDTO
    {
        public sealed record ProcesoItemDTO(
            int IdEleccion,
            string NombreEleccion,
            int IdProceso,
            string NombreProceso,
            string Tipodocumento,
            string NumeroDocumento,
            DateTime FechaFin
        );
        public sealed record CandidatoItemDTO(
            int IdEleccion,
            int IdProceso,
            int IdCandidato,
            string NombreCompleto,
            string Area,
            string UrlFile
        )
        {
            public string FotoPreview { get; init; } = string.Empty;
        };

        public sealed record VotantePageResponse(
            IReadOnlyCollection<ProcesoItemDTO> Procesos,
            IReadOnlyCollection<CandidatoItemDTO> Candidatos
        );

    }
    

    
}
