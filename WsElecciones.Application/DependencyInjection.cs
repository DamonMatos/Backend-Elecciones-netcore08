using FluentValidation;
using WsElecciones.Application.DTOs.PagoAsbanc;
using WsElecciones.Application.Features;
using WsElecciones.Application.Map;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace WsElecciones.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMapster();
        MapperConfiguration.Configure();
        services.AddScoped<PagoCuentaCorrienteHandler>();
        services.AddScoped<ProgramacionCuentaCorrienteHandler>();
        services.AddScoped<LoginHandler>(); 
        services.AddScoped<UserHandler>();
        services.AddScoped<EleccionesHandler>();
        services.AddScoped<ClienteHandler>();
        services.AddValidatorsFromAssemblyContaining<CreatePagoAsbancValidator>();

        return services;
    }
}
