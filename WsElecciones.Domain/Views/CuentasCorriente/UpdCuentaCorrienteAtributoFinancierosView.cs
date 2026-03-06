using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.CuentasCorriente
{
    public sealed record UpdCuentaCorrienteAtributoFinancierosView
    (
        string Estado,
        string Resultado
    );
}
