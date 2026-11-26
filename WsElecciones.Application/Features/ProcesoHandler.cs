using MapsterMapper;
using WsElecciones.Application.DTOs;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;

namespace WsElecciones.Application.Features
{
    public class ProcesoHandler(IUnitOfWork unitOfWork)
    {
        public async Task<Response<ResponseDTO>> DeleteAsync(int IdEleccion, int IdProceso, CancellationToken cancellationToken = default)
        {
            var result = await unitOfWork.EleccionesRepository.DeleteProceso(IdEleccion, IdProceso, cancellationToken);
            if (result.Estado == 0)
            {
                return Response<ResponseDTO>.Failure(result.Mensaje ?? "Error al eliminar el proceso.", Array.Empty<string>());
            }

            var response = new ResponseDTO(result.Id, result.Estado, result.Mensaje);
            return Response<ResponseDTO>.Ok(response);
        }
    }
}
