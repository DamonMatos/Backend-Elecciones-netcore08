using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public class GetDeudaCuentaCorrienteDTO
    {
        public sealed record CuentaCorrienteRequestDTO(
        string CodigoProducto,
        string TipoDocumento,
        string NumDocumento,
        string CodigoBanco,
        string Origen
        );

        public sealed record CuentaCorrienteResponseDTO(
        CuentaCorrienteDetalleDTO[]? Data);

        public sealed record CuentaCorrienteDetalleDTO(
            string NumDocumento,
            string CodigoProducto,
            string DescDocumento,
            DateTime? FechaVencimiento,
            DateTime? FechaEmision,
            double? Deuda,
            double? Mora,
            double? GastosAdm,
            double? PagoMinimo,
            string Periodo,
            int? Anio,
            string Cuota,
            string MonedaDoc);
    }
}
