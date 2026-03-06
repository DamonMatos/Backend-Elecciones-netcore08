namespace WsElecciones.Domain.Views.CuentasCorriente;

public sealed record ReiniciaCuentaCorrienteResultView(
    string? TipoAlumnoApto,
    string Reinicio,
    string Estado,
    string Resultado);
