namespace WsElecciones.Application.DTOs.ProgramacionesCuentaCorriente;

public static class CreateProgramacionCuentaCorrienteDTO
{
    public sealed record FacultadCarreraDTO(
        string CodFacultad,
        string? CodCarrera);

    public sealed record CreateProgramacionCuentaCorrienteRequestDTO(
        string CodNivel,
        string CodPeriodoAcademicoCtaCte,
        string CodCompania,
        string CodUsuario,
        DateOnly FechaDeseada,
        int CodOperacionProgramacionCtaCte = 1,
        IReadOnlyCollection<string>? CodDepartamentos = null,
        IReadOnlyCollection<string>? CodCampus = null,
        IReadOnlyCollection<FacultadCarreraDTO>? FacultadCarreras = null,
        IReadOnlyCollection<string>? CodAlumnos = null,
        IReadOnlyCollection<string>? CodAlumnosExcluir = null);

}
