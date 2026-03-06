
namespace WsElecciones.Domain.Views.Auth
{
    public sealed record GetAuthView(
        string? Token,
        DateTime? Expiry,
        IReadOnlyCollection<UserDto> User,
        IReadOnlyCollection<MenuDto> Menu
    );

    public sealed record UserDto(
        int IdUsuario,
        int IdPersonal,
        string ApePatPer,
        string ApeMatPer,
        string NomPer,
        string TipDocPer,
        string NumDocPer,
        string FehNacPer,
        string FotPer,
        int IdPerfil,
        string Perfil,
        string Correo,
        string ClaveHash
        );

    public sealed record MenuDto(
        int IdPerfil,
        string Perfil,
        int IdMenu,
        string Menu,
        int IdSubMenu,
        string NomVista,
        string NomUrl,
        string Icono,
        string Tipo,
        int Cantidad
        );

}


