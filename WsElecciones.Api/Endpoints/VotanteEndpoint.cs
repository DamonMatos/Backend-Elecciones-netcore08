using DocumentFormat.OpenXml.Office2010.Excel;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Colaboradores;
using WsElecciones.Application.DTOs.Elecciones;
using WsElecciones.Application.DTOs.Elector;
using WsElecciones.Application.DTOs.Votante;
using WsElecciones.Application.Features;

namespace WsElecciones.Api.Endpoints
{
    public static class VotanteEndpoint
    {
        public static RouteGroupBuilder MapVotanteEndpoints(this IEndpointRouteBuilder app) 
        {
            var group = app.MapGroup("/api/v1/votante")
            .WithTags("Votante")
            .WithOpenApi(); 

            group.MapEndpoint<GetVotanteDTO.VotantePageResponse>(
                HttpMethodType.Get,
                "{id:int}",
                "ObtenerVotantePorId", 
                async (
                    int id,
                    VotanteHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.GetByIdVotantesAsync(id, cancellationToken)
                                               .ConfigureAwait(false);

                    return response.Success
                        ? Results.Ok(response)
                        : Results.BadRequest(response);
                },
                new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }
            );

            group.MapEndpoint<ResponseDTO>(
                HttpMethodType.Post,
                string.Empty,
                "CrearElector",
                async (
                    CreateElectorDTO request,
                    ElectorHandler handler, 
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.CreateAsync(request, cancellationToken)
                                               .ConfigureAwait(false);

                    return response.Success
                        ? Results.Ok(response)
                        : Results.BadRequest(response);
                },
                new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }
            );

            return group;

        }
    }
}
