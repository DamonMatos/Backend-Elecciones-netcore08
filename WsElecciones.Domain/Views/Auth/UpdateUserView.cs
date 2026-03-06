using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Domain.Views.Auth
{
    public class UpdateUserView
    {
        public int IdUsuario { get; set; }
        public int IdPersonal { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string NombreFoto { get; set; }
        public string? NombreCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Ruc { get; set; }
    }
}
