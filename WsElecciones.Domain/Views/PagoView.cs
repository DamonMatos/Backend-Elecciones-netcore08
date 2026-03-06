using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views
{
    public static class PagoView
    {
        public sealed record PagoItem(
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

