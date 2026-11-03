using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.Elecciones
{
    public sealed record ProcesoView(
        int IdEleccion,
        int IdProceso,
        string Nombre,
        int NumeroCandidato,
        bool VotacionObligatoria,
        int Estado
    );
}
