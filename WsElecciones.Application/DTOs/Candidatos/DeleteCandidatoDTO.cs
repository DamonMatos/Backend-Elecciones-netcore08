using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Candidatos
{
    public sealed record DeleteCandidatoDTO
    {
        public int Accion { get; set; }
        public int IdEleccion { get; set; }
        public int IdProceso { get; set; }
        public int IdCandidato { get; set; }
    }
}
