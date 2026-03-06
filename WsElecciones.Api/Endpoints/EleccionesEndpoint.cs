using System.Security.Claims;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Application.DTOs.Elecciones;
using WsElecciones.Application.Features;

namespace WsElecciones.Api.Endpoints
{
    public static class EleccionesEndpoint
    {
        public static RouteGroupBuilder MapEleccionesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/elecciones").WithTags("Elecciones");

            group.MapEndpoint<EleccionesPagedResponseDTO>(
                HttpMethodType.Get,
                String.Empty,
                "ListaElecciones",
                async (
                    [AsParameters] EleccionesRequestDTO request,
                    HttpContext httpContext,
                    EleccionesHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.GetElecciones(request, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                        return Results.Unauthorized();

                    return Results.Ok(response);
                },
            new EndpointOptions { RequireValidation = false, NotRequiredCompania = true }

            ).RequireTokenAndRole("Administrador","Empresa");

            return group;
        }
    }
}


//var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); 
//var userRole = httpContext.User.FindFirstValue(ClaimTypes.Role);        
//var correo = httpContext.User.FindFirstValue(ClaimTypes.Email);
