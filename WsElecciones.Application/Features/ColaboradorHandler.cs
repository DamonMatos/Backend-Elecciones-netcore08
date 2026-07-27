using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Candidatos;
using WsElecciones.Application.DTOs.Colaboradores;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;
using WsElecciones.Domain.Views.Colaboradores;
using WsElecciones.Domain.Views.Elecciones;

namespace WsElecciones.Application.Features
{
    public class ColaboradorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        public async Task<Response<GetColaboradoresDTO.ColaboradoresPagedResponse>> GetColaboradorAsync(int page, int limit, int idEleccion, int idProceso, CancellationToken cancellationToken = default)
        {
            var result = await unitOfWork.ColaboradorRepository.GetColaboradoresAsync(
                page,
                limit,
                idEleccion,
                idProceso,
                cancellationToken)
                .ConfigureAwait(false);

            var items = result.Items
            .Select(item => new GetColaboradoresDTO.ColaboradorItemDTO(
                item.IdEleccion,
                item.IdProceso,
                item.TipoDocumento,
                item.NumeroDocumento,
                item.Cargo,
                item.Sede,
                item.Nombre,
                item.ApellidoPaterno,
                item.ApellidoMaterno,
                item.FechaRegistro,
                item.EmailDifusion,
                item.IdUsuario,
                item.Estado
                )).ToArray();

            var responseData = new GetColaboradoresDTO.ColaboradoresPagedResponse(
            items,
            result.TotalRegistros,
            result.Page,
            result.Limit);

            return Response<GetColaboradoresDTO.ColaboradoresPagedResponse>.Ok(responseData);

        }

        public async Task<Response<ResponseDTO>> DeleteColaboradorAsync(DeleteColaboradorDTO request, CancellationToken cancellationToken = default)
        {
            var result = await unitOfWork.ColaboradorRepository.DeleteColaboradorAsync(mapper.Map<DeleteColaboradorView>(request),cancellationToken);
            if (result.Estado == 0)
            {
                return Response<ResponseDTO>.Failure(result.Mensaje ?? "Error al eliminar el colaborador.", Array.Empty<string>());
            }

            var response = new ResponseDTO(result.Id, result.Estado, result.Mensaje);
            return Response<ResponseDTO>.Ok(response);
        }

        public async Task<Response<ResponseDTO>> CreateColaboradoresAsync(CrearListaColaboradorDTO request, CancellationToken cancellationToken = default)
        {
            if(request.Colaboradores is null) {
                return Response<ResponseDTO>.Failure("No se cargo ningun colaborador.", Array.Empty<string>());
            }

            var result = await unitOfWork.ColaboradorRepository.CrearColaboradoresAsync(mapper.Map<CrearListaColaborador>(request), cancellationToken);
            if (result.Estado == 0)
            {
                return Response<ResponseDTO>.Failure(result.Mensaje ?? "Error al cargar los colaboradores.", Array.Empty<string>());
            }

            var response = new ResponseDTO(result.Id, result.Estado, result.Mensaje);
            return Response<ResponseDTO>.Ok(response);
        }

    }
}
