using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.CuentasCorriente
{
    public sealed record UpdCuentaCorrienteAtributoFinancieros(
    int CodCuentaCorriente,
    int CodTipoDocumentoFinanciero,
    int CantidadCuotas,
    int CodEmpresa,
    string RazonSocial,
    string Usuario);
}
