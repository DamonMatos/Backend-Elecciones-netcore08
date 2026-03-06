using WsElecciones.Application.DTOs.Pago;
using WsElecciones.Application.DTOs.PagoAsbanc;
using WsElecciones.Application.Features;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.CrossCutting;
using WsElecciones.Api.Extensions;
namespace WsElecciones.Api.Endpoints;

public static class PagoCuentaCorrienteEndpoint
{
    public static RouteGroupBuilder MapPagoIntegracionesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/pagos/integraciones")
            .WithTags("Pagos");
        group.MapEndpoint<CreatePagoAsbancDTO.CreatePagoAsbancResponseDTO>(
            HttpMethodType.Post,
            "asbanc/pagos",
            "CrearPago",
            async (CreatePagoAsbancDTO.CreatePagoAsbancRequestDTO request, PagoCuentaCorrienteHandler handler, CancellationToken token) =>
            {
                var response = await handler.CrearPagoAsbancAsync(request).ConfigureAwait(false);
                if (!response.Success)
                {
                    return Results.BadRequest(response);
                }
                return Results.Created(string.Empty, response);
            },
            new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }
        );
        group.MapEndpoint<AnularPagoAsbancDTO.AnularPagoAsbancResponseDTO>(
           HttpMethodType.Post,
           "asbanc/anulaciones",
           "AnularPagoAsbanc",
           async (AnularPagoAsbancDTO.AnularPagoAsbancRequestDTO request, PagoCuentaCorrienteHandler handler, CancellationToken token) =>
           {
               var response = await handler.AnularPagoAsbancAsync(request).ConfigureAwait(false);
               if (!response.Success)
               {
                   return Results.BadRequest(response);
               }
               return Results.Created(string.Empty, response);
           },
           new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }
       );
        group.MapEndpoint<GetDeudaCuentaCorrienteDTO.CuentaCorrienteResponseDTO>(
            HttpMethodType.Post,
            "asbanc/deudas",
            "ConsultaDeuda",
            async (GetDeudaCuentaCorrienteDTO.CuentaCorrienteRequestDTO request, PagoCuentaCorrienteHandler handler, CancellationToken token) =>
            {
                var response = await handler.GetConsultaDeudaAsync(request).ConfigureAwait(false);
                if (!response.Success)
                {
                    return Results.BadRequest(response);
                }
                return Results.Ok(response);
            },
            new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }
        );
        group.MapPost("contipay/pagos", async (
                CreatePagoCuentaCorrienteDTO.CreatePagoCuentaCorrienteRequestDTO request,
                PagoCuentaCorrienteHandler handler,
                CancellationToken cancellationToken) =>
        {

        })
            .WithName("PagoCuentaCorriente")
            .NotRequiredCompania()
            .Produces<CreatePagoCuentaCorrienteDTO.CreatePagoCuentaCorrienteResponseDTO>(StatusCodes.Status201Created)
            .Produces<CreatePagoCuentaCorrienteDTO.CreatePagoCuentaCorrienteResponseDTO>(StatusCodes.Status400BadRequest);

        return group;
    }

    public static RouteGroupBuilder MapPagoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/pagos")
            .WithTags("Pagos");
        group.MapEndpoint<RevertirPagoAsbancDTO.RevertirPagoAsbancResponseDTO>(
            HttpMethodType.Post,
            "reversion-pagos-asbanc",
            "RevertirPagoAsbanc",
            async (RevertirPagoAsbancDTO.RevertirPagoAsbancRequestDTO request, PagoCuentaCorrienteHandler handler, CancellationToken token) =>
            {
                var response = await handler.RevertirPagoAsbancAsync(request).ConfigureAwait(false);
                if (!response.Success)
                {
                    return Results.BadRequest(response);
                }
                return Results.Created(string.Empty, response);
            },
            new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }
        );

        group.MapEndpoint<GetPagoDTO.PagoDto>(
            HttpMethodType.Get,
            "documentos/{id}",
            "PagoDocumento",
            async (int id, PagoCuentaCorrienteHandler handler, CancellationToken token) =>
            {
                var response = await handler
                .GetPagoAsync(id)
                .ConfigureAwait(false);
                if (!response.Success)
                {
                    return Results.BadRequest(response);
                }
                return Results.Created(string.Empty, response);
            },
            new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }
        );


        return group;
    }

}
