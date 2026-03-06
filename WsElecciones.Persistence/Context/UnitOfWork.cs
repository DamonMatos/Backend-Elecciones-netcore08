using WsElecciones.Domain;
using WsElecciones.Domain.Interface;
using WsElecciones.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WsElecciones.Persistence.Context;

public class UnitOfWork : IUnitOfWork
{
    public ICuentaCorrienteRepository CuentaCorrienteRepository { get; }
    public ICuentaCorrienteCuotaRepository CuentaCorrienteCuotaRepository { get; }
    public IPagoAsbancRepository PagoAsbancRepository { get; }
    public IPagoRepository PagoRepository { get; }
    public IProgramacionCuentaCorrienteRepository ProgramacionCuentaCorrienteRepository { get; }
    public IAuthRepository AuthRepository { get; }
    public IEleccionesRepository EleccionesRepository { get; }

    private readonly CuentaCorrienteContext _context;

    private readonly IDbConnection dbConnection;
    public IDbTransaction BeginTransaction()
    {
        if (dbConnection.State == ConnectionState.Closed)
        {
            dbConnection.Open();
        }

        return dbConnection.BeginTransaction();
    }

    public UnitOfWork(CuentaCorrienteContext _context,
        ICuentaCorrienteRepository cuentaCorrienteRepository,
        ICuentaCorrienteCuotaRepository cuentaCorrienteCuotaRepository,
        IPagoAsbancRepository pagoAsbancRepository,
        IProgramacionCuentaCorrienteRepository programacionCuentaCorrienteRepository,
        IPagoRepository pagoRepository,
        IAuthRepository authRepository,
        IEleccionesRepository eleccionesRepository
    )
    {
        this._context = _context ?? throw new ArgumentNullException(nameof(_context));
        dbConnection = _context.Database.GetDbConnection();
        CuentaCorrienteRepository = cuentaCorrienteRepository;
        PagoAsbancRepository = pagoAsbancRepository;
        ProgramacionCuentaCorrienteRepository = programacionCuentaCorrienteRepository;
        CuentaCorrienteCuotaRepository = cuentaCorrienteCuotaRepository;
        PagoRepository = pagoRepository;
        AuthRepository = authRepository;
        EleccionesRepository = eleccionesRepository;
    }
}
