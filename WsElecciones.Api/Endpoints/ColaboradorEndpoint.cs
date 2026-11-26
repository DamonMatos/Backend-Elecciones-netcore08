using Azure;
using Microsoft.AspNetCore.Mvc;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Application.DTOs.Colaboradores;
using WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;
using WsElecciones.Application.Features;
using static WsElecciones.Application.DTOs.Colaboradores.GetColaboradoresDTO;

namespace WsElecciones.Api.Endpoints
{
    public static class ColaboradorEndpoint
    {
        public static RouteGroupBuilder MapColaboradorEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/colaborador")
            .WithTags("Colaborador");

            group.MapEndpoint<GetColaboradoresDTO.ColaboradoresPagedResponse>(
                HttpMethodType.Get,
                string.Empty,
                "ListaColaboradores",
                async (
                    ColaboradorHandler handler,
                    CancellationToken cancellationToken,
                    int page = 1,
                    int limit = 10,
                    int idEleccion= 0,
                    int idProceso = 0
                ) =>
                {
                    var response = await handler
                        .GetColaboradorAsync(
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


            group.MapEndpoint<GetColaboradoresDTO.ColaboradorItemDTO>(
                HttpMethodType.Post,
                string.Empty,
                "CrearColaboradores",
                async (
                    CrearListaColaboradorDTO request,
                    ColaboradorHandler handler,
                    CancellationToken cancellationToken
                    ) =>
                {
                    var response = await handler.CreateColaboradoresAsync(request, cancellationToken).ConfigureAwait(false);
                    return response.Success ? Results.Ok(response) : Results.BadRequest(response);

                }, new EndpointOptions { RequireValidation = true, NotRequiredCompania = true })
                .RequireTokenAndRole("Administrador", "Empresa");


            group.MapEndpoint<GetColaboradoresDTO.ColaboradorItemDTO>(
                HttpMethodType.Delete,
                string.Empty, 
                "EliminarColaborador",
                async (
                    [FromForm] DeleteColaboradorDTO request,
                    ColaboradorHandler handler,
                    CancellationToken cancellationToken
                    ) =>
                {
                    var response = await handler.DeleteColaboradorAsync(request, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                    {
                        return Results.BadRequest(response);
                    }
                    return Results.Ok(response);
                }, new EndpointOptions { RequireValidation = true, NotRequiredCompania = true })
                .RequireTokenAndRole("Administrador", "Empresa");

            return group;

        }
    }
}
