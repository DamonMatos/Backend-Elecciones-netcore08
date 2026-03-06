using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public class CreatePagoCuentaCorrienteDTO
    {
        public sealed record CreatePagoCuentaCorrienteRequestDTO(
        long CodCuentaCorriente,
        long CodCuentaCorrienteCuota,
        int MedioPago,
        double ImporteCancelado
        );

        public sealed record CreatePagoCuentaCorrienteResponseDTO(
        bool Success,
        string Message,
        IReadOnlyCollection<string> Validations,
        PagoCuentaCorrienteDTO? Data);

        public sealed record PagoCuentaCorrienteDTO(
            int CodCuentaCorriente);
    }
}
