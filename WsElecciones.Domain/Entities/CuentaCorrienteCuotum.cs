using System;
using System.Collections.Generic;

namespace WsElecciones.Domain.Entitites;

public partial class CuentaCorrienteCuotum
{
    public int CodCuentaCorrienteCuota { get; set; }

    public string? CodNivel { get; set; }

    public int? CodConcepto { get; set; }

    public short? Item { get; set; }

    public decimal? CantidadCreditos { get; set; }

    public int? CantidadCursos { get; set; }

    public decimal? Importe { get; set; }

    public decimal? ImporteImpuesto { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? UsuarioModificacion { get; set; }

    public short? Cantidad { get; set; }

    public short? NumeroCuota { get; set; }

    public int? CodCuentaCorriente { get; set; }

    public DateTime? FechaProrroga { get; set; }

    public decimal? ImporteCancelado { get; set; }

    public string? VigenteCuentaCorrienteCuota { get; set; }

    public int? CodMoneda { get; set; }

    public decimal? TipoCambio { get; set; }

    public virtual CuentaCorriente? CodCuentaCorrienteNavigation { get; set; }
}
