using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views
{
    public sealed record ResponseView(
        int Estado,
        string Mensaje
        );
}
