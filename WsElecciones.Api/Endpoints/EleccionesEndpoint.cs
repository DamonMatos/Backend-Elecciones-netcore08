using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Elecciones;
using WsElecciones.Application.Features;

namespace WsElecciones.Api.Endpoints
{
    public static class EleccionesEndpoint
    {
        public static RouteGroupBuilder MapEleccionesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/elecciones").WithTags("Elecciones").RequireAuthorization(); 

            group.MapEndpoint<EleccionesPagedResponseDTO>(
                HttpMethodType.Get,
                String.Empty,
                "ObtenerEleccion",
                async (
                    [AsParameters] EleccionesRequestDTO request,
                    HttpContext httpContext,
                    EleccionesHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.GetAllAsync(request, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                        return Results.Unauthorized();

                    return Results.Ok(response);
                },

                new EndpointOptions { RequireValidation = false, NotRequiredCompania = true }

            ).RequireTokenAndRole("Administrador","Empresa");


            group.MapEndpoint<EleccionesDTO>(
                HttpMethodType.Get,
                "getById",
                "ObtenerEleccionById",
                async (
                    int IdCliente,
                    int IdEleccion,
                    HttpContext httpContext,
                    EleccionesHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.GetByIdAsync(IdCliente, IdEleccion, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                        return Results.Unauthorized();

                    return Results.Ok(response);
                },
            
                new EndpointOptions { RequireValidation = false, NotRequiredCompania = true }

            ).RequireTokenAndRole("Administrador", "Empresa");

            //EndPoint Eliminar Proceso
            group.MapEndpoint<ResponseDTO>(
                HttpMethodType.Delete,
                "delete",
                "EliminarProceso",
                async (
                    int IdEleccion,
                    int IdProceso,
                    HttpContext httpContext,
                    ProcesoHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.DeleteAsync(IdEleccion, IdProceso, cancellationToken).ConfigureAwait(false);
                    if (!response.Success)
                        return Results.Unauthorized();
                    return Results.Ok(response);
                },
                new EndpointOptions { RequireValidation = false, NotRequiredCompania = true }

            ).RequireTokenAndRole("Administrador", "Empresa");

            group.MapEndpoint<ResponseDTO>(
                HttpMethodType.Post,
                String.Empty, 
                "CrearEleccion", 
                async (
                    [FromForm] CreateEleccionesTemporalDTO request,
                    EleccionesHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.CreateAsync(request, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                    {
                        return Results.BadRequest(response);
                    }
                    return Results.Ok(response);
                },
                new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }

            ).Accepts<ResponseDTO>("multipart/form-data").RequireTokenAndRole("Administrador", "Empresa").DisableAntiforgery();

            group.MapEndpoint<ResponseDTO>(
                HttpMethodType.Post,
                "generarDifusion",
                "GenerarDifusion",
                async (
                    int IdEleccion,
                    DifusionHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.CreateAsync(IdEleccion, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                    {
                        return Results.BadRequest(response);
                    }
                    return Results.Ok(response);
                },
                    new EndpointOptions { RequireValidation = false, NotRequiredCompania = true }

                ).RequireTokenAndRole("Administrador", "Empresa");

            return group;
        }
    }
}

