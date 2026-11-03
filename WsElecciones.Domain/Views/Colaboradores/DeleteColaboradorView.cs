using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.Colaboradores
{
    public sealed record DeleteColaboradorView(
        int idEleccion,
        int idProceso,
        string tipoDocumento,
        string numeroDocumento
        );
}
