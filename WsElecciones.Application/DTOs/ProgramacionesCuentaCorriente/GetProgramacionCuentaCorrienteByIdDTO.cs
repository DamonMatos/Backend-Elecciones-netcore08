using System;
using System.Collections.Generic;

namespace WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;

public static class GetProgramacionCuentaCorrienteByIdDTO
{
    public sealed record ProgramacionCuentaCorrienteDetailResponse(
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
}
