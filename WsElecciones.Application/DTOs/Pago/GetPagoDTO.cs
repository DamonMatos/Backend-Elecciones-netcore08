using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Pago
{
    public class GetPagoDTO
    {
        public sealed record PagoDto(
        //int CodigoCancelacion,
        int CodigoMedioCancelacion,
        string? MedioPago,
        int CodigoMoneda,
        string? Moneda,
        string? CodigoBanco,
        decimal Importe,
        string? FechaCancelacion,
        bool EsTotal,
        string? NumeroOperacionBancaria,
        string? CodCuentaBancarioContinental,
        string? Banco,
        string? CanalPago,
        int NumeroDocumento,
        string? Serie,
        int? CodigoDocumento
       );
    }
}
