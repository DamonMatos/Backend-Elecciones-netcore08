using Azure.Core;
using MapsterMapper;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Auth;
using WsElecciones.Application.DTOs.Elecciones;
using WsElecciones.Application.DTOs.PagoAsbanc;
using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Storage;
using WsElecciones.Domain;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.Elecciones;

namespace WsElecciones.Application.Features
{
    public class EleccionesHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorage)
    {
        public async Task<Response<EleccionesPagedResponseDTO>> GetAllAsync(EleccionesRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await unitOfWork.EleccionesRepository.GetAllAsync(request.IdPersonal,request.Page,request.Limit, cancellationToken).ConfigureAwait(false);

            var items = result.Items
            .Select(item => new EleccionesResponseDTO(
                item.IdEleccion,
                item.IdCliente,
                item.RazonSocial,
                item.RUC,
                item.Nombre,
                item.ColorBase,
                item.UrlLogo,
                item.NumDocPer,
                item.FechaDifusion,
                item.FechaInicio,
                item.FechaFin,
                item.FechaRegistro,
                item.PlanillaConfirmada,
                item.DifusionEnviada,
                item.Estado))
            .ToArray();

            var responseData = new EleccionesPagedResponseDTO(
                items,
                result.TotalRegistros,
                result.Page,
                result.Limit);

            return Response<EleccionesPagedResponseDTO>.Ok(responseData);

        }

        public async Task<Response<EleccionesDTO>> GetByIdAsync(int IdCliente, int IdEleccion, CancellationToken cancellationToken = default) 
        {
            var result = await unitOfWork.EleccionesRepository.GetByIdAsync(IdCliente, IdEleccion, cancellationToken).ConfigureAwait(false);

            var eleccion = new EleccionesResponseDTO(
                result.Eleccion.IdEleccion,
                result.Eleccion.IdCliente,
                result.Eleccion.RazonSocial,
                result.Eleccion.RUC,
                result.Eleccion.Nombre,
                result.Eleccion.ColorBase,
                result.Eleccion.UrlLogo,
                result.Eleccion.NumDocPer,
                result.Eleccion.FechaDifusion,
                result.Eleccion.FechaInicio,
                result.Eleccion.FechaFin,
                result.Eleccion.FechaRegistro,
                result.Eleccion.PlanillaConfirmada,
                result.Eleccion.DifusionEnviada,
                result.Eleccion.Estado
            );

            var procesos = result.Procesos
            .Select(item => new ProcesoResponseDTO(
                item.IdEleccion,
                item.IdProceso,
                item.Nombre,
                item.NumeroCandidato,
                item.VotacionObligatoria,
                item.Estado))
            .ToArray();

            var responseData = new EleccionesDTO(
                eleccion,
                procesos);

            return Response<EleccionesDTO>.Ok(responseData);
        }

        public async Task<Response<ResponseDTO>> CreateAsync(CreateEleccionesTemporalDTO request,CancellationToken cancellationToken = default)
        {
            string modulo = string.Empty;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var procesos = JsonSerializer.Deserialize<List<CreateProcesoDTO>>(request.Procesos, options) ?? [];

            var view = new CreateEleccionesDTO(
                request.IdEleccion,
                request.IdCliente,
                request.Nombre,
                request.ColorBase,
                request.FechaInicio,
                request.FechaFin,
                request.PlanillaConfirmada,
                request.DifusionEnviada,
                request.Estado,
                procesos,
                request.FechaDifusion
            );       

            var result = await unitOfWork.EleccionesRepository.CreateAsync(mapper.Map<CreateEleccionesView>(view), cancellationToken);

            if (result.Estado == 0)
                return Response<ResponseDTO>.Failure(result.Mensaje ?? "Error al guardar la elección.",Array.Empty<string>());
      
            string filename = string.Empty;
            modulo = "Elecciones";

            if (request.Logo is not null)
            {
                var extension = Path.GetExtension(request.Logo.FileName).ToLowerInvariant();
                if (extension != ".jpg")
                {
                    return Response<ResponseDTO>.Failure("Solo se permiten archivos.jpg", Array.Empty<string>());
                }
                
                filename = result.Id.ToString() + extension.Trim();
                
                await fileStorage.SaveAsync(modulo, result.Id.ToString(), filename, request.Logo, cancellationToken);
            }

            //Metodo para crear Carpetas de Proceso
            var ListaProceso = await unitOfWork.EleccionesRepository.GetAllProcesoAsync(result.Id, cancellationToken);

            if (ListaProceso.Count > 0) {
                foreach (var idProceso in ListaProceso)
                {
                   await fileStorage.CreateCarpeta(modulo, result.Id.ToString(), idProceso.ToString());
                }
            }
           
            var response = new ResponseDTO(result.Id, result.Estado, result.Mensaje);
            return Response<ResponseDTO>.Ok(response);
        }      

    }
}
