namespace WsElecciones.Domain.Views.CuentaCorrienteCuotas
{
    public sealed record GetCuotasCuentaCorrienteView(
        int CodCuentaCorrienteCuota,
        int NumeroCuota,
        string FechaPago,
        string CodMoneda,
        string CodMonedaInternacional,
        decimal Importe,
        decimal Impuesto,
        decimal TotalPagado,
        decimal BeneficiosTotales,
        decimal TotalAPagar,
        decimal Saldo,
        string Vigente,
        int TieneCancelacion);
}
