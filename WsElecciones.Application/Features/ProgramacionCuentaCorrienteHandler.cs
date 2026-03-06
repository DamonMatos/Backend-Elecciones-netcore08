using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentValidation;
using WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;
using WsElecciones.Domain.Views;
using MapsterMapper;

namespace WsElecciones.Application.Features;

public class ProgramacionCuentaCorrienteHandler
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProgramacionCuentaCorrienteDTO.CreateProgramacionCuentaCorrienteRequestDTO> _validator;
    private const int ExcelExportLimit = 999_999_999;

    public ProgramacionCuentaCorrienteHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IValidator<CreateProgramacionCuentaCorrienteDTO.CreateProgramacionCuentaCorrienteRequestDTO> validator)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Response<int>> CreateProgramacionCuentaCorriente(
        CreateProgramacionCuentaCorrienteDTO.CreateProgramacionCuentaCorrienteRequestDTO request)
    {
        var codProgramacionCtaCte = await _unitOfWork.ProgramacionCuentaCorrienteRepository
            .CreateProgramacionCuentaCorrienteAsync(_mapper.Map<CreateProgramacionCuentaCorriente>(request))
            .ConfigureAwait(false);

        return Response<int>.Ok(
            codProgramacionCtaCte,
            "Programación de cuenta corriente creada correctamente");
    }

    public async Task<Response<GetProgramacionCuentaCorrienteDTO.ProgramacionCuentaCorrientePagedResponse>>GetProgramacionCuentaCorrienteAsync(
            int page,
            int limit,
            string? codNivel = null,
            string? codPeriodoAcademicoCtaCte = null,
            int? codTipoOperacion = null,
            CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();
        if (page <= 0)
        {
            errors.Add("El número de página debe ser mayor a cero.");
        }

        if (limit <= 0)
        {
            errors.Add("El tamaño de página debe ser mayor a cero.");
        }

        if (errors.Count > 0)
        {
            return Response<GetProgramacionCuentaCorrienteDTO.ProgramacionCuentaCorrientePagedResponse>.Failure(
                "Parámetros de paginación inválidos",
                errors);
        }

        var result = await _unitOfWork.ProgramacionCuentaCorrienteRepository
            .GetProgramacionCuentaCorrienteAsync(
                page,
                limit,
                codNivel,
                codPeriodoAcademicoCtaCte,
                codTipoOperacion,
                cancellationToken)
            .ConfigureAwait(false);

        var items = result.Items
            .Select(item => new GetProgramacionCuentaCorrienteDTO.ProgramacionCuentaCorrienteItemDTO(
                item.Lote,
                item.UsuarioSolicitante,
                item.FechaDeseada,
                item.FechaEjecucion,
                item.EstadoEjecucion,
                item.ColorWeb,
                item.CantidadAlumnosProcesados,
                item.CantidadAlumnosSatisfactorios,
                item.AlumnosErrados))
            .ToArray();

        var responseData = new GetProgramacionCuentaCorrienteDTO.ProgramacionCuentaCorrientePagedResponse(
            items,
            result.TotalRegistros,
            result.Page,
            result.Limit);

        return Response<GetProgramacionCuentaCorrienteDTO.ProgramacionCuentaCorrientePagedResponse>.Ok(responseData);
    }

    public async Task<Response<GetProgramacionCuentaCorrienteByIdDTO.ProgramacionCuentaCorrienteDetailResponse>>
        GetProgramacionCuentaCorrienteByIdAsync(
            int codProgramacionCtaCte,
            string codCompania,
            CancellationToken cancellationToken = default)
    {
        if (codProgramacionCtaCte <= 0)
        {
            return Response<GetProgramacionCuentaCorrienteByIdDTO.ProgramacionCuentaCorrienteDetailResponse>.Failure(
                "Identificador inválido",
                new[] { "El identificador de la programación debe ser mayor a cero." });
        }

        if (string.IsNullOrWhiteSpace(codCompania))
        {
            return Response<GetProgramacionCuentaCorrienteByIdDTO.ProgramacionCuentaCorrienteDetailResponse>.Failure(
                "Código de compañía inválido",
                new[] { "El código de compañía es obligatorio." });
        }

        var detail = await _unitOfWork.ProgramacionCuentaCorrienteRepository
            .GetProgramacionCuentaCorrienteByIdAsync(
                codProgramacionCtaCte,
                codCompania,
                cancellationToken)
            .ConfigureAwait(false);

        if (detail is null)
        {
            return Response<GetProgramacionCuentaCorrienteByIdDTO.ProgramacionCuentaCorrienteDetailResponse>.Failure(
                "Programación de cuenta corriente no encontrada",
                Array.Empty<string>());
        }

        var response = new GetProgramacionCuentaCorrienteByIdDTO.ProgramacionCuentaCorrienteDetailResponse(
            detail.Lote,
            detail.FechaDeseada,
            detail.CodNivel,
            detail.CodPeriodoAcademicoCtaCte,
            detail.CodEstadoProgramacionCtaCte,
            detail.EstadoProgramacionCtaCte,
            detail.ColorWeb,
            detail.CantidadAlumnosProcesados,
            detail.CantidadAlumnosErrados,
            detail.Departamentos,
            detail.Campus,
            detail.Facultades,
            detail.Carreras,
            detail.codOperationType,
            detail.operationType);

        return Response<GetProgramacionCuentaCorrienteByIdDTO.ProgramacionCuentaCorrienteDetailResponse>.Ok(response);
    }

    public async Task<Response<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoPagedResponse>>
        GetProgramacionCuentaCorrienteAlumnosAsync(
            int codProgramacionCtaCte,
            string codCompania,
            int page,
            int limit,
            bool estadoOk = true,
            CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();
        if (codProgramacionCtaCte <= 0)
        {
            errors.Add("El identificador de la programación debe ser mayor a cero.");
        }

        if (string.IsNullOrWhiteSpace(codCompania))
        {
            errors.Add("El código de compañía es obligatorio.");
        }

        if (page <= 0)
        {
            errors.Add("El número de página debe ser mayor a cero.");
        }

        if (limit <= 0)
        {
            errors.Add("El tamaño de página debe ser mayor a cero.");
        }

        if (errors.Count > 0)
        {
            return Response<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoPagedResponse>.Failure(
                "Parámetros de consulta inválidos",
                errors);
        }

        var result = await _unitOfWork.ProgramacionCuentaCorrienteRepository
            .GetProgramacionCuentaCorrienteAlumnosAsync(
                codProgramacionCtaCte,
                codCompania.Trim(),
                page,
                limit,
                estadoOk,
                cancellationToken)
            .ConfigureAwait(false);

        var items = result.Items
            .Select(item => new GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoItemDTO(
                item.CodAlumno,
                item.ApellidosNombres,
                item.Carrera,
                item.Modalidad,
                item.Campus,
                item.DescripcionIncidente))
            .ToArray();

        var responseData = new GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoPagedResponse(
            items,
            result.TotalRegistros,
            result.Page,
            result.Limit);

        return Response<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoPagedResponse>.Ok(responseData);
    }

    public async Task<Response<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnosExcelResponse>>
        GetProgramacionCuentaCorrienteAlumnosExcelAsync(
            int codProgramacionCtaCte,
            string codCompania,
            CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();
        if (codProgramacionCtaCte <= 0)
        {
            errors.Add("El identificador de la programación debe ser mayor a cero.");
        }

        if (string.IsNullOrWhiteSpace(codCompania))
        {
            errors.Add("El código de compañía es obligatorio.");
        }

        if (errors.Count > 0)
        {
            return Response<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnosExcelResponse>.Failure(
                "Parámetros de consulta inválidos",
                errors);
        }

        var trimmedCodCompania = codCompania.Trim();

        var exportResult = await _unitOfWork.ProgramacionCuentaCorrienteRepository
            .GetProgramacionCuentaCorrienteAlumnosExportAsync(
                codProgramacionCtaCte,
                trimmedCodCompania,
                ExcelExportLimit,
                cancellationToken)
            .ConfigureAwait(false);

        var exitososItems = exportResult.Exitosos
            .Select(item => new GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoItemDTO(
                item.CodAlumno,
                item.ApellidosNombres,
                item.Carrera,
                item.Modalidad,
                item.Campus,
                item.DescripcionIncidente))
            .ToArray();

        var conErroresItems = exportResult.ConErrores
            .Select(item => new GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnoItemDTO(
                item.CodAlumno,
                item.ApellidosNombres,
                item.Carrera,
                item.Modalidad,
                item.Campus,
                item.DescripcionIncidente))
            .ToArray();

        var responseData = new GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnosExcelResponse(
            exitososItems,
            conErroresItems);

        return Response<GetProgramacionCuentaCorrienteAlumnosDTO.ProgramacionCuentaCorrienteAlumnosExcelResponse>.Ok(responseData);
    }

    public async Task<Response<AnularProgramacionCuentaCorrienteDTO.AnularProgramacionCuentaCorrienteResponse>>
        AnularProgramacionCuentaCorrienteAsync(
            int codProgramacionCtaCte,
            string codCompania,
            string codUsuario,
            CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();
        if (codProgramacionCtaCte <= 0)
        {
            errors.Add("El identificador de la programación debe ser mayor a cero.");
        }

        if (string.IsNullOrWhiteSpace(codCompania))
        {
            errors.Add("El código de compañía es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(codUsuario))
        {
            errors.Add("El código de usuario es obligatorio.");
        }

        if (errors.Count > 0)
        {
            return Response<AnularProgramacionCuentaCorrienteDTO.AnularProgramacionCuentaCorrienteResponse>.Failure(
                "Parámetros inválidos para anular la programación de cuenta corriente",
                errors);
        }

        var estadoAnulacion = await _unitOfWork.ProgramacionCuentaCorrienteRepository
            .AnularProgramacionCuentaCorrienteAsync(
                codProgramacionCtaCte,
                codCompania.Trim(),
                codUsuario.Trim(),
                cancellationToken)
            .ConfigureAwait(false);

        var responseData = new AnularProgramacionCuentaCorrienteDTO.AnularProgramacionCuentaCorrienteResponse(estadoAnulacion);

        if (estadoAnulacion == 1)
        {
            return Response<AnularProgramacionCuentaCorrienteDTO.AnularProgramacionCuentaCorrienteResponse>.Ok(
                responseData,
                "Programación de cuenta corriente anulada correctamente.");
        }

        var details = new[]
        {
            "La programación no se encuentra en estado pendiente o ya fue procesada."
        };

        return new Response<AnularProgramacionCuentaCorrienteDTO.AnularProgramacionCuentaCorrienteResponse>(
            false,
            "No se pudo anular la programación de cuenta corriente.",
            responseData,
            details);
    }
}
