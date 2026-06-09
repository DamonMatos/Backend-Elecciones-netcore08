using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Elecciones
{
    public sealed record ProcesoResponseDTO(
        int IdEleccion,
        int IdProceso, 
        string Nombre,
        int NumeroCandidato, 
        bool VotacionObligatoria,
        int Estado
        );

    public sealed record GetProceoDTO(
        int IdProceso
    );
}
