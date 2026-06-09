using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Candidatos;

namespace WsElecciones.Domain.Interface
{
    public interface ICandidatoRepository
    {
        Task<GetCandidatosView.CandidatoPagedResult> GetCandidatosAsync(
            int page,
            int limit,
            int idEleccion,
            int idProceso,
            CancellationToken cancellationToken = default);

        Task<ResponseView> CreateCandidatosAsync(int accion,GetCandidatosView.CandidatoItem entidad, CancellationToken cancellationToken = default);

    }
}
