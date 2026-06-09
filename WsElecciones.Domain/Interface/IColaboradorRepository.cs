using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Views.Colaboradores;

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
    }
}
