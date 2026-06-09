using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Application.DTOs.Candidatos;
using WsElecciones.Application.DTOs.Colaboradores;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;

namespace WsElecciones.Application.Features
{
    public class ColaboradorHandler(IUnitOfWork unitOfWork)
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

    }
}
