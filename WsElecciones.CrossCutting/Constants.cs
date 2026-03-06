using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.CrossCutting
{
    public static class Constants
    {
        public static bool IsInDevelopment { get; set; } = false;
        public static string QueryString { get; set; } = "";
        public static string RouteParams { get; set; } = "";
        public static string TraceIdentifier { get; set; } = "";
        public static string RequestBody { get; set; } = "";
        public static string Host { get; set; } = null!;
        public static string CodUsuario { get; set; } = null!;
        public static string? CodCompania { get; set; }
        public static string JWT { get; set; } = "";

        public const string AppName = "ApiCtaCte";
    }
}
