using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Elecciones;
using WsElecciones.Application.Features;

namespace WsElecciones.Api.Endpoints
{
    public static class CatalogoEndpoint
    {
        public static RouteGroupBuilder MapCatalogoEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/catalogo").WithTags("Catalogo");

            group.MapEndpoint<SelectItemDto>(
                HttpMethodType.Get,
                String.Empty,
                "ObtenerCatalogo",
                async (
                    int tipo,
                    int id,
                    HttpContext httpContext,
                    SelectItemHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.GetByIdAsync(tipo,id, cancellationToken).ConfigureAwait(false);
                    return response.Success ? Results.Ok(response) : Results.NotFound(response);
                },
            new EndpointOptions { RequireValidation = false, NotRequiredCompania = true }
            );

            return group;

        }
    }
}
