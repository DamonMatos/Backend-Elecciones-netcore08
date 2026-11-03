using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.Colaboradores
{
    public sealed record CrearColaboradoresView(
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

    public sealed record CrearListaColaborador(
        IReadOnlyCollection<CrearColaboradoresView> Colaboradores
        );
}
