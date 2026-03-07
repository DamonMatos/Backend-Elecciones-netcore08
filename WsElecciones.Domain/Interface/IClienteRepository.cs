using System;
using WsElecciones.Domain.Views.Cliente;

namespace WsElecciones.Domain.Interface
{
    public interface IClienteRepository
    {
        public Task<IReadOnlyCollection<ClienteView>> GetAsync(int id, CancellationToken cancellationToken = default);
        public Task<ClienteView> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    }
}
