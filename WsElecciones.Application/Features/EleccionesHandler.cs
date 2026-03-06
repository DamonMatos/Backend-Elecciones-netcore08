using WsElecciones.Application.DTOs.Elecciones;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;

namespace WsElecciones.Application.Features
{
    public class EleccionesHandler(IUnitOfWork unitOfWork)
    {
        public async Task<Response<EleccionesPagedResponseDTO>> GetElecciones(EleccionesRequestDTO request, CancellationToken cancellationToken = default)
        {
            var result = await unitOfWork.EleccionesRepository.GetEleccionesAsysc(request.IdPersonal,request.page,request.limit, cancellationToken).ConfigureAwait(false);

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

    }
}
