using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace WsElecciones.Domain.Views;

public class CreateProgramacionCuentaCorriente
{
    public string CodNivel { get; set; } = default!;
    public string CodPeriodoAcademicoCtaCte { get; set; } = default!;

    public string CodCompania { get; set; } = default!;
    public string CodUsuario { get; set; } = default!;

    public DateOnly FechaDeseada { get; set; } = default!;
    public int CodOperacionProgramacionCtaCte { get; set; } = 1;
    public IReadOnlyCollection<string> CodDepartamentos { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> CodCampus { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<FacultadCarrera> FacultadCarreras { get; set; } = Array.Empty<FacultadCarrera>();
    public IReadOnlyCollection<string> CodAlumnos { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> CodAlumnosExcluir { get; set; } = Array.Empty<string>();

    public sealed record FacultadCarrera(string CodFacultad, string? CodCarrera);
}
