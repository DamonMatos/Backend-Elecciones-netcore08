using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Entities
{
    public partial class PagoAsbanc
    {
        public int CodPagoAsbanc { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string CanalPago { get; set; }
        public string CodigoBanco { get; set; }
        public string NumOperacionBanco { get; set; }
        public string FormaPago { get; set; }
        public string TipoConsulta { get; set; }
        public string IdConsulta { get; set; }
        public string CodigoProducto { get; set; }
        public string NumDocumento { get; set; }
        public double ImportePagado { get; set; }
        public string MonedaDoc { get; set; }
        public string Procesado { get; set; }
        public DateTime? FechaProceso { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
