using System.Collections.Generic;
using System.IO;
using System.Threading;
using ClosedXML.Excel;
using WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;
using WsElecciones.Application.Features;
using WsElecciones.CrossCutting;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WsElecciones.Api.Endpoints;

public static class ProgramacionCuentaCorrienteEndpoint
{
    public static RouteGroupBuilder MapProgramacionCuentaCorrienteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/programacion-cuenta-corriente")
            .WithTags("Programación Cuenta Corriente");

        group.MapEndpoint<GetProgramacionCuentaCorrienteDTO.ProgramacionCuentaCorrientePagedResponse>(
            HttpMethodType.Get,
            string.Empty,
            "ListarProgramacionCuentaCorriente",
            async (
                ProgramacionCuentaCorrienteHandler handler,
                CancellationToken cancellationToken,
                int page = 1,
                int limit = 10,
                string? codNivel = null,
                string? codPeriodoAcademico = null,
                int? codTipoOperacion = null
            ) =>
            {
                var response = await handler
                    .GetProgramacionCuentaCorrienteAsync(
                        page,
                        limit,
                        codNivel,
                        codPeriodoAcademico,
                        codTipoOperacion,
                        cancellationToken)
                    .ConfigureAwait(false);

                if (!response.Success)
                {
                    return Results.BadRequest(response);
                }

                return Results.Ok(response);
            }, new EndpointOptions { RequireValidation = true });

        group.MapEndpoint<int>(
            HttpMethodType.Post,
            string.Empty,
            "CrearProgramacionCuentaCorriente",
            async (
                CreateProgramacionCuentaCorrienteDTO.CreateProgramacionCuentaCorrienteRequestDTO request,
                ProgramacionCuentaCorrienteHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler.CreateProgramacionCuentaCorriente(request)
                    .ConfigureAwait(false);

                if (!response.Success)
                {
                    return Results.BadRequest(response);
                }

                return Results.Ok(response);
            }, new EndpointOptions { }
        );

        group.MapEndpoint<GetProgramacionCuentaCorrienteByIdDTO.ProgramacionCuentaCorrienteDetailResponse>(
            HttpMethodType.Get,
            "{id:int}", "ObtenerProgramacionCuentaCorrientePorId", async (
                int id,
                ProgramacionCuentaCorrienteHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler
                    .GetProgramacionCuentaCorrienteByIdAsync(
                        id,
                        Constants.CodCompania,
                        cancellationToken)
                    .ConfigureAwait(false);

                if (!response.Success)
                {
                    if (response.Errors.Count == 0)
                    {
                        return Results.NotFound(response);
                    }

                    return Results.BadRequest(response);
                }

                return Results.Ok(response);
            }, new EndpointOptions { RequireValidation = true });

        group.MapEndpoint<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoPagedResponse>(
            HttpMethodType.Get,
            "{id:int}/alumnos",
            "ListarAlumnosProgramacionCuentaCorriente",
            async (
                int id,
                ProgramacionCuentaCorrienteHandler handler,
                int page = 1,
                int limit = 10,
                string? estadoOk = null,
                CancellationToken cancellationToken = default) =>
            {
                var estadoOkValue =
                    estadoOk is null || bool.TryParse(estadoOk, out var parsedEstadoOk) && parsedEstadoOk;

                var response = await handler
                    .GetProgramacionCuentaCorrienteAlumnosAsync(
                        id,
                        Constants.CodCompania,
                        page,
                        limit,
                        estadoOkValue,
                        cancellationToken)
                    .ConfigureAwait(false);

                if (!response.Success)
                {
                    return Results.BadRequest(response);
                }

                return Results.Ok(response);
            }, new EndpointOptions { RequireValidation = true });


        group.MapEndpoint<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnosExcelResponse>(
            HttpMethodType.Get, "{id:int}/alumnos/excel", "DescargarAlumnosProgramacionCuentaCorriente", async (
                int id,
                ProgramacionCuentaCorrienteHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler
                    .GetProgramacionCuentaCorrienteAlumnosExcelAsync(
                        id,
                        Constants.CodCompania ?? string.Empty,
                        cancellationToken)
                    .ConfigureAwait(false);

                if (!response.Success || response.Data is null)
                {
                    return Results.BadRequest(response);
                }

                var fileName = $"programacion-cta-cte-alumnos-{id}.xlsx";
                var fileBytes = BuildProgramacionCuentaCorrienteAlumnosWorkbook(response.Data);

                return Results.File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }, new EndpointOptions { RequireValidation = true });


        group.MapEndpoint<AnularProgramacionCuentaCorrienteDTO.AnularProgramacionCuentaCorrienteResponse>(
            HttpMethodType.Post,
            "{id:int}/anular",
            "AnularProgramacionCuentaCorriente",
            async (
                int id,
                ProgramacionCuentaCorrienteHandler handler,
                CancellationToken cancellationToken) =>
            {
                var response = await handler
                    .AnularProgramacionCuentaCorrienteAsync(
                        id,
                        Constants.CodCompania,
                        Constants.CodUsuario,
                        cancellationToken)
                    .ConfigureAwait(false);

                if (!response.Success)
                {
                    return Results.BadRequest(response);
                }

                return Results.Ok(response);
            }, new EndpointOptions { RequireValidation = true });

        return group;
    }

    private static byte[] BuildProgramacionCuentaCorrienteAlumnosWorkbook(
        GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnosExcelResponse data)
    {
        using var workbook = new XLWorkbook();
        var exitososSheet = workbook.Worksheets.Add("Exitosos");
        FillWorksheet(exitososSheet, data.Exitosos);

        var conErroresSheet = workbook.Worksheets.Add("Con errores");
        FillWorksheet(conErroresSheet, data.ConErrores);

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    private static void FillWorksheet(
        IXLWorksheet worksheet,
        IReadOnlyCollection<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoItemDTO> items)
    {
        var headers = new[]
        {
            "Código Alumno",
            "Apellidos y Nombres",
            "Carrera",
            "Modalidad",
            "Campus",
            "Descripción Incidente"
        };

        for (var index = 0; index < headers.Length; index++)
        {
            worksheet.Cell(1, index + 1).Value = headers[index];
        }

        worksheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var item in items)
        {
            worksheet.Cell(row, 1).Value = item.CodAlumno;
            worksheet.Cell(row, 2).Value = item.ApellidosNombres ?? string.Empty;
            worksheet.Cell(row, 3).Value = item.Carrera ?? string.Empty;
            worksheet.Cell(row, 4).Value = item.Modalidad ?? string.Empty;
            worksheet.Cell(row, 5).Value = item.Campus ?? string.Empty;
            worksheet.Cell(row, 6).Value = item.DescripcionIncidente ?? string.Empty;
            row++;
        }

        worksheet.Columns().AdjustToContents();
    }
}