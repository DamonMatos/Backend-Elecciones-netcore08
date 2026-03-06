using System.Collections.Generic;

namespace WsElecciones.Domain.Views;

public class CuentaCorrienteView
{
    public sealed record CuentaCorriente(
        long? CodCuentaCorriente,
        IReadOnlyCollection<string> validations,
        IReadOnlyCollection<CuentaCorrienteCuota> Cuotas,
        string result);
    
    public sealed record CuentaCorrienteCuota(
        long? CodCuentaCorrienteCuota,
        int? NumeroCuota,
        int? CodConcepto,
        string? Concepto,
        decimal? Cantidad,
        string? Moneda,
        decimal? TasaImporte,
        decimal? TasaImpuesto,
        DateTime? FechaVencimiento,
        DateTime? FechaProrroga,
        IList<CuentaCorrienteBeneficio> Beneficios);
    
    public sealed record CuentaCorrienteBeneficio(
        long CodCuentaCorrienteCuota,
        int? CodBeneficio,
        string? Beneficio,
        decimal? Importe,
        string? Origen,
        bool? Mandatorio,
        int CodConcepto);

    public sealed record CuentaCorrienteCuotaResumen(
        int? CodCuentaCorrienteCuota,
        string? Item,
        string? VigenteCuentaCorrienteCuota,
        int? NumeroCuota,
        decimal? ImporteTotal,
        decimal? ImporteBeneficio,
        decimal? ImporteImpuesto,
        decimal? TotalAPagar,
        decimal? ImporteCancelado,
        decimal? Saldo);
    
    public sealed record CuentaCorrienteConceptoResumen(
        string? VigenteCuentaCorrienteCuota,
        int? CodConcepto,
        string? Concepto,
        string? Beneficio,
        int? NumeroCuota,
        decimal? ImporteTotal,
        decimal? ImporteBeneficio,
        decimal? ImporteImpuesto,
        decimal? TotalAPagar,
        decimal? ImporteCancelado,
        decimal? Saldo);
    
    public sealed record CuentaCorrienteCancelacionResumen(
        int? CodCancelacion,
        int? CodMedioCancelacion,
        string? MedioCancelacion,
        int? CodTipoDocumentoFinanciero,
        string? DocumentoFinanciero,
        string? NumeroDocumento,
        int? CodCanalPago,
        string? CanalPago,
        int? CodBanco,
        string? Banco,
        int? CodDocumentoNc,
        string? NumeroDocumentoNc,
        string? Moneda,
        decimal? ImporteCancelado,
        DateTime? FechaCancelacion);

    public sealed record CuentaCorrienteDetail(
        string? Alumno,
        string? CodCuentaCorriente,
        string? Vigente,
        string? Nivel,
        string? Carrera,
        string? Modalidad,
        string? PeriodoAcademico,
        string? Campus,
        string? Escala,
        int? CantidadCreditos,
        int? CantidadCursos,
        string? CategoriaPago,
        decimal? PrecioCargaAcademica,
        string? TipoDocumento,
        int? CodTipoDocumento,
        string? Empresa,
        string? CodEmpresa,
        int? CantidadCuotas);
    
    
    public sealed record CuentaCorrienteResumen(
        long CodCuentaCorriente,
        long? CodNotaContable,
        decimal? MontoPagado,
        decimal? SaldoAPagar);

    public sealed record ResumenCuentaCorrienteItem(
        long CodCuentaCorriente,
        string? CodPeriodoAcademico,
        string? DescripcionCarrera,
        string? DescripcionDepartamento,
        string? DescripcionCampus,
        string? VigenteCuentaCorriente,
        decimal? ImporteCarga,
        string? CodTipoDocumentoFinanciero,
        string? DocumentoFinanciero,
        string? CodTasaCuotas,
        int? CantidadCuotas,
        int? CantidadCreditos,
        int? CantidadCursos,
        decimal? ImporteTotal,
        decimal? ImporteBeneficio,
        decimal? ImporteImpuesto,
        decimal? TotalAPagar,
        decimal? ImporteCancelado,
        decimal? Saldo);

    public sealed record ResumenCuentaCorrientePagedResult(
        IReadOnlyCollection<ResumenCuentaCorrienteItem> Items,
        int TotalRegistros,
        int Page,
        int Limit);
}
