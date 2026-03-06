using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.PagoAsbanc
{
    public class AnularPagoAsbancDTO
    {
        public sealed record AnularPagoAsbancRequestDTO(
        DateTime FechaTransaccion,
        string CodigoBanco,
        string NumOperacionBanco,
        string TipoConsulta,
        string IdConsulta,
        string NumOperacionBancoERP);

        public sealed record AnularPagoAsbancResponseDTO(
        PagoAsbancAnuladoDTO? Data);

        public sealed record PagoAsbancAnuladoDTO(
            int NumOperacionERP,
            string NombreAlumno);

    }
}
