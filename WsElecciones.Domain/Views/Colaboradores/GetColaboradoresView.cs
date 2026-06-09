using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WsElecciones.Domain.Views.ProgramacionCuentaCorrienteView;

namespace WsElecciones.Domain.Views.Colaboradores
{
    public class GetColaboradoresView
    {
        public sealed record ColadoradorItem(
            int IdEleccion,
            int IdProceso,
            string TipoDocumento,
            string NumeroDocumento,
            string Cargo,
            string Sede,
            string Nombre,
            string ApellidoPaterno,
            string ApellidoMaterno,
            DateTime? FechaRegistro,
            string EmailDifusion,
            int IdUsuario,
            int Estado
            );

        public sealed record ColaboradoresPagedResult(
            IReadOnlyCollection<ColadoradorItem> Items,
            int TotalRegistros,
            int Page,
            int Limit
            );

    }
}
