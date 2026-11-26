using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WsElecciones.Domain.Views.CreateProgramacionCuentaCorriente;

namespace WsElecciones.Domain.Views.Elecciones
{
    public class CreateEleccionesView
    {
       public int IdEleccion { get; set; }
       public int IdCliente { get; set; }
       public string Nombre { get; set; }
       public string ColorBase {get;set;}
       public DateTime FechaInicio {get;set;}
       public DateTime FechaFin {get;set;}
       public bool PlanillaConfirmada {get;set;}
       public bool DifusionEnviada {get;set;}
       public int Estado {get;set;}
        public IReadOnlyCollection<CreateProcesoView> Procesos { get; set; }
       public DateTime? FechaDifusion { get; set; }
    }
}
