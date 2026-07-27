using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.Elecciones;

namespace WsElecciones.Domain.Interface
{
    public interface IEleccionesRepository
    {
        Task<EleccionesPagedResult> GetAllAsync(int IdPersonal,int Page,int Limit,CancellationToken cancellationToken = default);
        Task<ResponseView> CreateAsync(CreateEleccionesView request, CancellationToken cancellationToken= default);
        Task<EleccionView> GetByIdAsync(int IdCliente, int IdEleccion, CancellationToken cancellationToken = default);
        Task<ResponseView> DeleteProceso(int IdEleccion, int IdProceso, CancellationToken cancellationToken = default);
        Task<ResponseView> GenerarDifusion(int IdEleccion, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<int>> GetAllProcesoAsync(int IdEleccion, CancellationToken cancellationToken = default);
    }
}
