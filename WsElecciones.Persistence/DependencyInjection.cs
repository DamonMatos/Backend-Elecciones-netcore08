using WsElecciones.Domain;
using WsElecciones.Domain.Interface;
using WsElecciones.Persistence.Context;
using WsElecciones.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace WsElecciones.Persistence;

public static class DependencyInjection
{
    private static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

    public static IServiceCollection AddInfraestructureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistenceDependency(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IStoredProcedureExecutor, StoredProcedureExecutor>();
        services.AddScoped<ICuentaCorrienteRepository, CuentaCorrienteRepository>();
        services.AddScoped<IPagoAsbancRepository, PagoAsbancRepository>();
        services.AddScoped<IProgramacionCuentaCorrienteRepository, ProgramacionCuentaCorrienteRepository>();
        services.AddScoped<ICuentaCorrienteCuotaRepository, CuentaCorrienteCuotaRepository>();
        services.AddTransient<ICuentaCorrienteRepository, CuentaCorrienteRepository>();
        services.AddTransient<IPagoAsbancRepository, PagoAsbancRepository>();        
        services.AddTransient<ICuentaCorrienteRepository, CuentaCorrienteRepository>();
        services.AddTransient<IProgramacionCuentaCorrienteRepository, ProgramacionCuentaCorrienteRepository>();
        services.AddTransient<IPagoRepository, PagoRepository>();
        services.AddTransient<IClienteRepository, ClienteRepository>();
        services.AddTransient<IEleccionesRepository, EleccionesRepository>();
        services.AddTransient<IAuthRepository, AuthRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddPersistenceDependency(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CuentaCorrienteContext>(x =>
        {
            x.UseLoggerFactory(loggerFactory);
            x.EnableSensitiveDataLogging();
            x.EnableDetailedErrors().LogTo(Console.WriteLine, LogLevel.Debug);

            x.UseSqlServer(GetConnectionString(configuration));
            x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<CuentaCorrienteContext>();
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<CuentaCorrienteContext>());

        return services;
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        return configuration.GetConnectionString("DefaultConnection")
               ?? throw new ArgumentNullException("configuration",
                   "database connection DefaultConnection doesn't exists in AppSettings");
    }
}
