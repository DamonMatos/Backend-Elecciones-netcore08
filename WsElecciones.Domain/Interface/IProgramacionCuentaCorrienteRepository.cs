using System.Threading;
using WsElecciones.Domain.Entitites;
using WsElecciones.Domain.Views;

namespace WsElecciones.Domain.Interface;

public interface IProgramacionCuentaCorrienteRepository
{
    Task<int> CreateProgramacionCuentaCorrienteAsync(CreateProgramacionCuentaCorriente entidad);
    Task<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrientePagedResult> GetProgramacionCuentaCorrienteAsync(
        int page,
        int limit,
        string? codNivel = null,
        string? codPeriodoAcademicoCtaCte = null,
        int? codTipoOperacion = null,
        CancellationToken cancellationToken = default);
    Task<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteDetail?> GetProgramacionCuentaCorrienteByIdAsync(
        int codProgramacionCtaCte,
        string codCompania,
        CancellationToken cancellationToken = default);
    Task<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnoPagedResult> GetProgramacionCuentaCorrienteAlumnosAsync(
        int codProgramacionCtaCte,
        string codCompania,
        int page,
        int limit,
        bool estadoOk = true,
        CancellationToken cancellationToken = default);
    Task<ProgramacionCuentaCorrienteView.ProgramacionCuentaCorrienteAlumnosExportResult> GetProgramacionCuentaCorrienteAlumnosExportAsync(
        int codProgramacionCtaCte,
        string codCompania,
        int limit,
        CancellationToken cancellationToken = default);
    Task<int> AnularProgramacionCuentaCorrienteAsync(
        int codProgramacionCtaCte,
        string codCompania,
        string codUsuario,
        CancellationToken cancellationToken = default);
}
