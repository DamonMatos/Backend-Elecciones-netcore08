using System;
using WsElecciones.Domain.Views.Cliente;

namespace WsElecciones.Domain.Interface
{
    public interface IClienteRepository
    {
        //Este metodo falta incluir SP y EndPoint
        public Task<IReadOnlyCollection<ClienteView>> GetAsync(int idPersonal, CancellationToken cancellationToken = default);
        public Task<ClienteView> GetByIdAsync(int idPersonal, CancellationToken cancellationToken = default);

    }
}
