using System;
using System.Collections.Generic;

namespace WsElecciones.Domain.Entitites;

public partial class CuentaCorriente
{
    public int CodCuentaCorriente { get; set; }

    public string? CodNivel { get; set; }

    public string? CodAlumno { get; set; }

    public string? CodPeriodoAcademico { get; set; }

    public string? CodCarrera { get; set; }

    public string? CodDepartamento { get; set; }

    public string? CodCampus { get; set; }

    public string? IdSis1 { get; set; }

    public string? IdSis2 { get; set; }

    public int? CodCategoriaPagoDetallePeriodo { get; set; }

    public int? CodTasaCuotas { get; set; }

    public int? CodTipoDocumento { get; set; }

    public string? TipoAlumno { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? UsuarioModificacion { get; set; }

    public string? CodEmpresa { get; set; }

    public string? IdSis3 { get; set; }

    public string? IdSis4 { get; set; }

    public string? IdSis5 { get; set; }

    public string? VigenteCuentaCorriente { get; set; }

    public string? CodPeriodoAcademicoReferencia { get; set; }

    public string? Activo { get; set; }

    public int? CodPersona { get; set; }

    public virtual ICollection<CuentaCorrienteCuotum> CuentaCorrienteCuota { get; set; } =
        new List<CuentaCorrienteCuotum>();
}