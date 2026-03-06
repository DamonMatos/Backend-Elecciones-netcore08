using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public class CreatePagoAsbancDTO
    {
        public sealed record CreatePagoAsbancRequestDTO(
        DateTime FechaTransaccion,
        string CanalPago,
        string CodigoBanco,
        string NumOperacionBanco,
        string FormaPago,
        string TipoConsulta,
        string IdConsulta,
        string CodigoProducto,
        string NumDocumento,
        double ImportePagado,
        string MonedaDoc);

        public sealed record CreatePagoAsbancResponseDTO(
        bool Success,
        string Message,
        IReadOnlyCollection<string> Validations,
        PagoAsbancDTO? Data);

        public sealed record PagoAsbancDTO(
            int NumOperacionERP,
            string NombreAlumno);

    }
}
