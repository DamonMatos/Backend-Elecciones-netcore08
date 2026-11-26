using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.Elector
{
    public class CreateElectorView
    {
        public int IdUsuario { get; set; }
        public int IdEleccion { get; set; }
        public int IdProceso { get; set; }
        public int? IdCandidato1 { get; set; }
        public int? IdCandidato2 { get; set; }
        public int? IdCandidato3 { get; set; }
        public int? IdCandidato4 { get; set; }
        public int? IdCandidato5 { get; set; }
        public int? IdCandidato6 { get; set; }  
    }
}
