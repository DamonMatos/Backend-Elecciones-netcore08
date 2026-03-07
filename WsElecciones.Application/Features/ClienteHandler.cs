using MapsterMapper;
using WsElecciones.Application.DTOs.Auth;
using WsElecciones.Application.DTOs.Cliente;
using WsElecciones.Application.DTOs.Elecciones;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;

namespace WsElecciones.Application.Features
{
    public class ClienteHandler(IUnitOfWork unitOfWork)
    {
        public async Task<Response<IEnumerable<ClienteDto>>> GetAsync(int IdPersonal , CancellationToken cancellationToken) 
        {
            var request = await unitOfWork.ClienteRepository.GetAsync(IdPersonal, cancellationToken).ConfigureAwait(false);
            var items = request.Select(x => new ClienteDto(
                x.IdCliente,
                x.NombreCliente,
                x.EstadoCliente,
                x.RazonSocial,
                x.Ruc,
                x.IdPersonal)
            ).ToArray();

            return Response<IEnumerable<ClienteDto>>.Ok(items);
        }

        public async Task<Response<ClienteDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        { 
                var cliente = await unitOfWork.ClienteRepository
                    .GetByIdAsync(id, cancellationToken);

                if (cliente is null) {
                    return Response<ClienteDto>.Failure("Cliente no encontrado.", Array.Empty<string>());
                }
                   
                var dto = new ClienteDto(
                    cliente.IdCliente,
                    cliente.NombreCliente,
                    cliente.EstadoCliente,
                    cliente.RazonSocial,
                    cliente.Ruc,
                    cliente.IdPersonal
                );

                return Response<ClienteDto>.Ok(dto);
        }
    }
}
