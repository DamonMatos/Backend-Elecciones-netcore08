using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public class RevertirPagoAsbancDTO
    {
        public sealed record RevertirPagoAsbancRequestDTO(
            int CodDocumento,
            string Usuario);

        public sealed record RevertirPagoAsbancResponseDTO(
        PagoRevertidoDTO? Data);

        public sealed record PagoRevertidoDTO(
            int Resultado);
    }
}
