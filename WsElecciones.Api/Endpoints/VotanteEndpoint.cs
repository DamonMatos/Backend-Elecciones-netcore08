using DocumentFormat.OpenXml.Office2010.Excel;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Application.DTOs.Colaboradores;
using WsElecciones.Application.DTOs.Votante;
using WsElecciones.Application.Features;

namespace WsElecciones.Api.Endpoints
{
    public static class VotanteEndpoint
    {
        public static RouteGroupBuilder MapVotanteEndpoints(this IEndpointRouteBuilder app) 
        {
            var group = app.MapGroup("/api/v1/votante")
            .WithTags("Votante");

            group.MapEndpoint<GetVotanteDTO.VotantePageResponse>(
                HttpMethodType.Get,
                "{id:int}",
                "ListaVotantes",
                async (
                    VotanteHandler handler,
                    int id,
                    CancellationToken cancellationToken                    
                    ) => 
                {
                    var response = await handler.
                    GetByIdVotantesAsync(
                        id,
                        cancellationToken)
                    .ConfigureAwait(false);

                    if (!response.Success)
                    {
                        return Results.BadRequest(response);
                    }
                    return Results.Ok(response);
                },new EndpointOptions { RequireValidation = true, NotRequiredCompania = true });

            return group;

        }
    }
}
