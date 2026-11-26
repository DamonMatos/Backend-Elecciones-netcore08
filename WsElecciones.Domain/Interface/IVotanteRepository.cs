using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Views.Colaboradores;
using WsElecciones.Domain.Views.Votante;

namespace WsElecciones.Domain.Interface
{
    public interface IVotanteRepository
    {
        Task<GetVotanteView.VotantePageResult> GetColaboradoresAsync(
            int IdUsuario,
            CancellationToken cancellationToken = default);
    }
}
