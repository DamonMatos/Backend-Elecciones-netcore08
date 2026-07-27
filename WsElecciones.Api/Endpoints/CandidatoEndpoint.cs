using Microsoft.AspNetCore.Mvc;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Candidatos;
using WsElecciones.Application.DTOs.Colaboradores;
using WsElecciones.Application.Features;

namespace WsElecciones.Api.Endpoints
{
    public static class CandidatoEndpoint
    {
        public static RouteGroupBuilder MapCandidatoEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/candidato")
            .WithTags("Candidato");

            group.MapEndpoint<GetCandidatosDTO.CandidatoPagedResponse>(
                HttpMethodType.Get,
                string.Empty,
                "ListaCandidato",
                async (
                    CandidatoHandler handler,
                    CancellationToken cancellationToken,
                    int page = 1,
                    int limit = 10,
                    int idEleccion = 0,
                    int idProceso = 0
                ) =>
                {
                    var response = await handler
                        .GetCandidatoAsync(
                            page,
                            limit,
                            idEleccion,
                            idProceso,
                            cancellationToken)
                        .ConfigureAwait(false);

                    if (!response.Success)
                    {
                        return Results.BadRequest(response);
                    }

                    return Results.Ok(response);
                }, new EndpointOptions { RequireValidation = true, NotRequiredCompania= true });

            group.MapEndpoint<CreateCandidatoDTO>(
                HttpMethodType.Post,
                string.Empty,
                "CrearCandidato",
                async (
                    [FromForm] CreateCandidatoDTO request,
                    CandidatoHandler handler,
                    CancellationToken cancellationToken
                    ) => {
                        var response = await handler.AddCandidatoAsync(request, cancellationToken).ConfigureAwait(false);

                        if (!response.Success)
                        {
                            return Results.BadRequest(response);
                        }
                        return Results.Ok(response);
                    },
                    new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }

            ).Accepts<ResponseDTO>("multipart/form-data").RequireTokenAndRole("Administrador", "Empresa").DisableAntiforgery();

            return group;

        }
    }
}
