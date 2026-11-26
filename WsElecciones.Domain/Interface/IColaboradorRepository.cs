using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Colaboradores;
using WsElecciones.Domain.Views.Usuario;

namespace WsElecciones.Domain.Interface
{
    public interface IColaboradorRepository
    {
        Task<GetColaboradoresView.ColaboradoresPagedResult> GetColaboradoresAsync(
            int page, 
            int limit, 
            int idEleccion,
            int idProceso, 
            CancellationToken cancellationToken = default);

        Task<ResponseView> DeleteColaboradorAsync(
            DeleteColaboradorView request, 
            CancellationToken cancellationToken = default);

        Task<ResponseView> CrearColaboradoresAsync(
            CrearListaColaborador request,
            CancellationToken cancellationToken= default);

        Task<IReadOnlyCollection<GetColaboradoresView.ColaboradoresEnvioMasivoCorreo>> GetColaboradoresEnvioMasivoAsync(
            int idEleccion,
            CancellationToken cancellationToken = default
        );

        Task<ResponseView> UpdateUsuarioAsync(
            UpdateUsuarioView.UpdateUsuariosListRequest usuarios,
            CancellationToken cancellationToken = default
            );

    }
}
