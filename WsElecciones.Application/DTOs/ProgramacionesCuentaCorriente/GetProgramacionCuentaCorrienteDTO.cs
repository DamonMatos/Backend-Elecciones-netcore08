using System;
using System.Collections.Generic;

namespace WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;

public static class GetProgramacionCuentaCorrienteDTO
{
    public sealed record ProgramacionCuentaCorrienteItemDTO(
        int Lote,
        string? UsuarioSolicitante,
        DateTime? FechaDeseada,
        DateTime? FechaEjecucion,
        string? EstadoEjecucion,
        string? ColorWeb,
        int CantidadAlumnosProcesados,
        int CantidadAlumnosSatisfactorios,
        int AlumnosErrados);

    public sealed record ProgramacionCuentaCorrientePagedResponse(
        IReadOnlyCollection<ProgramacionCuentaCorrienteItemDTO> Items,
        int TotalRegistros,
        int Page,
        int Limit);
}
