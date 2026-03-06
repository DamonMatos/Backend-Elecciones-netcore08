using WsElecciones.Domain.Entitites;
using WsElecciones.Domain.Views;
using WsElecciones.Domain.Views.CuentasCorriente;

namespace WsElecciones.Domain.Interface;

public interface ICuentaCorrienteRepository : IRepository<CuentaCorriente>
{
    Task<UpdCuentaCorrienteAtributoFinancierosView> UpdCuentaCorrienteAtributoFinancierosAsync(UpdCuentaCorrienteAtributoFinancieros cuentaCorrienteAtributoFinancieros, CancellationToken cancellationToken = default);
    Task<CuentaCorrienteView.CuentaCorriente> CreateCuentaCorrienteAsync(CreateCuentaCorrienteView entidad, CancellationToken cancellationToken = default);
    Task<CuentaCorrienteView.CuentaCorriente> UpdateCuentaCorrienteAsync(UpdateCuentaCorrienteView entidad, CancellationToken cancellationToken = default);
    Task<PagedView<GetCuentasCorrienteView>> GetCuentasCorrienteByFilterAsync(int page, int limit,
        string codNivel,
        string codPeriodoAcademico,
        string codCompania,
        string? nombreAlumno = null,
        string? empresa = null,
        CancellationToken cancellationToken = default);

    Task<CuentaCorrienteView.CuentaCorrienteDetail?> GetCuentaCorrienteByIdAsync(
        string codCompania,
        int codCuentaCorriente,
        CancellationToken cancellationToken = default);

    Task<DeleteCuentaCorrienteResultView?> DeleteCuentaCorrienteAsync(
        int codCuentaCorriente,
        string motivoEliminacion,
        string usuario,
        CancellationToken cancellationToken = default);

    Task<ReiniciaCuentaCorrienteResultView?> ReiniciaCuentaCorrienteAsync(
        int codCuentaCorriente,
        int numeroCuotaDesde,
        string motivoReinicio,
        string usuario,
        CancellationToken cancellationToken = default);

    Task<ActualizacionMasivoView> UpdMasivoAsync(ActualizacionMasivo actualizacionMasivo, CancellationToken cancellationToken = default);

    Task<ActualizacionMasivoViewPagedResult> GetMasivoAsync(int page,int limit,string codCompania,string? estado,int? codEjecucion,CancellationToken cancellationToken = default);
}
