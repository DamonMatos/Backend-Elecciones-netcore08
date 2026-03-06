using System.Collections.Generic;

namespace WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;

public static class GetProgramacionCuentaCorrienteAlumnosDTO
{
    public sealed record ProgramacionCuentaCorrienteAlumnoItemDTO(
        string CodAlumno,
        string? ApellidosNombres,
        string? Carrera,
        string? Modalidad,
        string? Campus,
        string? DescripcionIncidente);

    public sealed record ProgramacionCuentaCorrienteAlumnoPagedResponse(
        IReadOnlyCollection<ProgramacionCuentaCorrienteAlumnoItemDTO> Items,
        int TotalRegistros,
        int Page,
        int Limit);

    public sealed record ProgramacionCuentaCorrienteAlumnosExcelResponse(
        IReadOnlyCollection<ProgramacionCuentaCorrienteAlumnoItemDTO> Exitosos,
        IReadOnlyCollection<ProgramacionCuentaCorrienteAlumnoItemDTO> ConErrores);
}
