using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.Application.DTOs.Colaboradores
{
    public class UpdateUsuarioDTO
    {
        public sealed record UpdateUsuarioItemRequest(
            string Correo,
            string ClaveHash
        );

        public sealed record UpdateUsuariosListRequest(
            IReadOnlyCollection<UpdateUsuarioItemRequest> Usuarios
        );
    }
}
