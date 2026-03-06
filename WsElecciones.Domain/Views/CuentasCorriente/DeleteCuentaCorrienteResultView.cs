namespace WsElecciones.Domain.Views.CuentasCorriente;

public sealed record DeleteCuentaCorrienteResultView(
    string? TipoAlumnoApto,
    string Elimino,
    string Estado,
    string Resultado);
