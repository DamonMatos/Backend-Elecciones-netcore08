using System.Data;
using WsElecciones.Domain.Interface;

namespace WsElecciones.Domain;

public interface IUnitOfWork
{
    ICuentaCorrienteRepository CuentaCorrienteRepository { get; }
    ICuentaCorrienteCuotaRepository CuentaCorrienteCuotaRepository { get; }
    IPagoAsbancRepository PagoAsbancRepository { get; }
    IProgramacionCuentaCorrienteRepository ProgramacionCuentaCorrienteRepository { get; }
    IPagoRepository PagoRepository { get; }
    IAuthRepository AuthRepository { get; }
    IEleccionesRepository EleccionesRepository { get; }
    IClienteRepository ClienteRepository { get; }
    IDbTransaction BeginTransaction();
}
