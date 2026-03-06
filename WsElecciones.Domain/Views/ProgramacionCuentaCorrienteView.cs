using System;
using System.Collections.Generic;

namespace WsElecciones.Domain.Views;

public static class ProgramacionCuentaCorrienteView
{
    public sealed record ProgramacionCuentaCorrienteItem(
        int Lote,
        string? UsuarioSolicitante,
        DateTime? FechaDeseada,
        DateTime? FechaEjecucion,
        string? EstadoEjecucion,
        string? ColorWeb,
        int CantidadAlumnosProcesados,
        int CantidadAlumnosSatisfactorios,
        int AlumnosErrados);

    public sealed record ProgramacionCuentaCorrientePagedResult(
        IReadOnlyCollection<ProgramacionCuentaCorrienteItem> Items,
        int TotalRegistros,
        int Page,
        int Limit);

    public sealed record ProgramacionCuentaCorrienteDetail(
        int Lote,
        DateTime? FechaDeseada,
        string? CodNivel,
        string? CodPeriodoAcademicoCtaCte,
        int? CodEstadoProgramacionCtaCte,
        string? EstadoProgramacionCtaCte,
        string? ColorWeb,
        int CantidadAlumnosProcesados,
        int CantidadAlumnosErrados,
        IReadOnlyCollection<string> Departamentos,
        IReadOnlyCollection<string> Campus,
        IReadOnlyCollection<string> Facultades,
        IReadOnlyCollection<string> Carreras,
        int codOperationType,
        string operationType);

    public sealed record ProgramacionCuentaCorrienteAlumnoItem(
        string CodAlumno,
        string? ApellidosNombres,
        string? Carrera,
        string? Modalidad,
        string? Campus,
        string? DescripcionIncidente);

    public sealed record ProgramacionCuentaCorrienteAlumnoPagedResult(
        IReadOnlyCollection<ProgramacionCuentaCorrienteAlumnoItem> Items,
        int TotalRegistros,
        int Page,
        int Limit);

    public sealed record ProgramacionCuentaCorrienteAlumnosExportResult(
        IReadOnlyCollection<ProgramacionCuentaCorrienteAlumnoItem> Exitosos,
        IReadOnlyCollection<ProgramacionCuentaCorrienteAlumnoItem> ConErrores);
}
