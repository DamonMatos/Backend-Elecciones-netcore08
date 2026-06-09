using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WsElecciones.Application.DTOs.Candidatos
{
    public class GetCandidatosDTO
    {
        public sealed record CandidatoItemDto(
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
            )
            {
                public string FotoPreview { get; init; } = string.Empty;
            };


        public sealed record CandidatoPagedResponse(
            IReadOnlyCollection<CandidatoItemDto> Items,
            int TotalRegistros,
            int Page,
            int Limit
            );
    }
}
