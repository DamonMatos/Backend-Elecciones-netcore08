using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views
{
    public class ConsultaDeudaCuentaCorrienteView
    {
        public sealed record ConsultaDeudaCuentaCorrienteItem(
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

        public sealed record ConsultaDeudaCuentaCorrienteResult(
            IReadOnlyCollection<ConsultaDeudaCuentaCorrienteItem> Items);
    }
}
