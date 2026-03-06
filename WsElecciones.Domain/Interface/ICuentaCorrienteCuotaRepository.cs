using WsElecciones.Domain.Entitites;
using WsElecciones.Domain.Views.CuentaCorrienteCuotas;

namespace WsElecciones.Domain.Interface
{
    public interface ICuentaCorrienteCuotaRepository: IRepository<CuentaCorrienteCuotum>
    {
        Task<IReadOnlyCollection<GetCuotasCuentaCorrienteView>> GetCuotasByCodigoCuentaCorrienteAsync(
            int? codCuentaCorriente, 
            CancellationToken cancellationToken = default
            );

        Task<IReadOnlyCollection<GetCuentaCorrienteConceptoResumenView>> GetConceptosByCuentaCorrienteCuotaIdAsync(
            string codCompania, 
            int codCuentaCorrienteCuota, 
            CancellationToken cancellationToken = default);
    }
}
