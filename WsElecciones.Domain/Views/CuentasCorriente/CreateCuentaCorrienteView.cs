namespace WsElecciones.Domain.Views.CuentasCorriente;

public class CreateCuentaCorrienteView
{
    public string CodNivel { get; set; }
    public string CodAlumno { get; set; }
    public string CodPeriodoAcademico { get; set; }
    public string CodCarrera { get; set; }
    public string CodDepartamento { get; set; }
    public string CodCampus { get; set; }
    public string CodFacultad { get; set; }
    public DateTime FechaInicioClases { get; set; }
    public string TipoAlumno { get; set; }
    //public string? Modulo { get; set; }
    public string? CodColegio { get; set; }
    public string? IdSis1 { get; set; }
    public string? IdSis2 { get; set; }
    public string? IdSis3 { get; set; }
    public string? IdSis4 { get; set; }
    public string? IdSis5 { get; set; }

    public long CodPersona { get; set; }
    public string? CodEmpresa { get; set; }
    public string CodCompania { get; set; }
    public string DatosPersonalesAlumno { get; set; }
    public string? DatosEmpresa { get; set; }
    public string DescripcionCarrera { get; set; }
    public string DescripcionDepartamento { get; set; }
    public string DescripcionCampus { get; set; }
    public string DescripcionPeriodoAcademico { get; set; }
    public string? DescripcionIdSis1 { get; set; }
    public string? DescripcionIdSis2 { get; set; }
    public string? DescripcionIdSis3 { get; set; }
    public string? DescripcionIdSis4 { get; set; }
    public string? DescripcionIdSis5 { get; set; }

    public IReadOnlyCollection<int>? OtrosCargos { get; set; }
    //public IReadOnlyCollection<CreditoMatriculado>? CreditosMatriculados { get; set; }
    public CreditoMatriculado? CreditosMatriculados { get; set; }
    public int? CodBeneficio {  get; set; }
    //public IReadOnlyCollection<int>? Beneficios { get; set; }
}

public class CreditoMatriculado
{
    public int CantidadCreditos { get; set; }
    public int? CantidadCursos { get; set; }
    public string? IdModulo { get; set; }
    public string? CodEmpresa {  get; set; }
    public string? DatosEmpresa { get; set; }
    public int? CodTasaCuotas { get; set; }
}
