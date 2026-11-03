using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.Elecciones
{
    public class CreateProcesoView
    {
        public int IdEleccion    {get;set;}
        public int IdProceso    {get;set;}
        public string Nombre { get;set;}
        public int NumeroCandidato     {get;set;}
        public bool VotacionObligatoria  {get;set;}
        public int Estado   {get;set;}
    }
}
