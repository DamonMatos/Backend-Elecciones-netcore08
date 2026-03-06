namespace WsElecciones.Domain.Views.CuentaCorrienteCuotas
{
    public sealed record GetCuentaCorrienteConceptoResumenView(
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
}
