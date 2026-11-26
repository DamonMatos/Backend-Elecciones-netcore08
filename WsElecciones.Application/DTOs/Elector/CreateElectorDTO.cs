using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Elector
{
    public sealed record CreateElectorDTO(
        int IdUsuario,
        int IdEleccion,
        int IdProceso,
        //DateTime FechaSufragio,
        int? IdCandidato1,
        int? IdCandidato2,
        int? IdCandidato3,
        int? IdCandidato4,
        int? IdCandidato5,
        int? IdCandidato6
        );
    
    
}
