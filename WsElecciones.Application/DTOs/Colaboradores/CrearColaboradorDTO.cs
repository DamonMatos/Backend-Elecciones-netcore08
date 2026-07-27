using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsElecciones.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WsElecciones.Application.DTOs.Colaboradores
{
    public sealed record CrearColaboradorDTO(
        int IdEleccion,
        int IdProceso,
        string Tipodocumento,
        string NumeroDocumento,
        string Cargo,
        string Sede,
        string Nombre,
        string Apellidopaterno,
        string Apellidomaterno,
        string Emaildifusion,
        int Estado
        );

    public sealed record CrearListaColaboradorDTO(
        IReadOnlyCollection<CrearColaboradorDTO> Colaboradores
        );
}
