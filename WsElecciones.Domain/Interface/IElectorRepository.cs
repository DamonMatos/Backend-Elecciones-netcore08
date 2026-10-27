using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Elector;

namespace WsElecciones.Domain.Interface
{
    public interface IElectorRepository
    {
        Task<ResponseView> CreateElectorAsync(CreateElectorView request, CancellationToken cancellationToken = default);
    }
}
