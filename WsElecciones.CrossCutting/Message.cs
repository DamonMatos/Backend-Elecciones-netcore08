using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.CrossCutting
{
    public static class Message
    {
        public static readonly string GenericErrorMessage = "Ocurrió un error y no se pudo realizar la operación solicitada";
        public static readonly string GenericSuccessRegisterMessage = "La operación solicitada finalizó correctamente";
        public static readonly string GenericNoDataFoundMessage = "No se encontró información.";
        public static readonly string UnexpectedErrorMessage = "Ocurrió un error en el sistema, contactar al equipo de soporte. <br><br> Código de error: {0}";
        public static readonly string InvalidCredentials = "Usuario y/o contraseña incorrecto.";
        public static readonly string InactiveUser = "Usuario inactivo.";
        public static readonly string ExpiredUser = "Usuario expirado.";
    }
}
