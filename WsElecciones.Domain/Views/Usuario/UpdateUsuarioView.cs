namespace WsElecciones.Domain.Views.Usuario
{
    public class UpdateUsuarioView 
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
