using Azure.Core;
using MapsterMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Application.DTOs.Candidatos;
using WsElecciones.Application.DTOs.Cliente;
using WsElecciones.Application.DTOs.Votante;
using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Storage;
using WsElecciones.Domain;

namespace WsElecciones.Application.Features
{
    public class VotanteHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<FileStorageConfig> storageOptions)
    {
        public async Task<Response<GetVotanteDTO.VotantePageResponse>> GetByIdVotantesAsync(int idUsuario, CancellationToken cancellationToken = default) {

            var result = await unitOfWork.VotanteRepository.GetColaboradoresAsync(
                idUsuario,
                cancellationToken)
                .ConfigureAwait(false);

            var storage = storageOptions.Value.Modules["Elecciones"];
            var baseUrl = storage.BaseUrl;

            var procesos = result.Procesos
           .Select(item => new GetVotanteDTO.ProcesoItemDTO(
               item.IdEleccion,
               item.NombreEleccion,
               item.IdProceso,
               item.NombreProceso,
               item.Tipodocumento,
               item.NumeroDocumento,
               item.FechaFin
           )).ToArray();

            var candidatos = result.Candidatos
           .Select(item => new GetVotanteDTO.CandidatoItemDTO(
               item.IdEleccion,
               item.IdProceso,
               item.IdCandidato,
               item.NombreCompleto,
               item.Area,
               item.UrlFile
           ) with
           {
               FotoPreview = string.IsNullOrWhiteSpace(item.UrlFile)
                   ? string.Empty
                   : $"{baseUrl}/{item.IdEleccion}/{item.IdProceso}/{item.UrlFile}"
           }).ToArray();

            var responseData = new GetVotanteDTO.VotantePageResponse(
                procesos,
                candidatos);

            return Response<GetVotanteDTO.VotantePageResponse>.Ok(responseData);
        }
    }
}
