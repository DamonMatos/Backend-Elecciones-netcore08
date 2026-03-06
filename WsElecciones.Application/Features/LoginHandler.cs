using MapsterMapper;
using WsElecciones.Application.DTOs.Auth;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.Auth;
using UserDto = WsElecciones.Domain.Views.Auth.UserDto;

namespace WsElecciones.Application.Features
{
    public class LoginHandler(IMapper mapper,IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        public async Task<Response<LoginResponseDTO>> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken= default)
        {
            var login = await unitOfWork.AuthRepository.GetByUsernameAsync(request.Correo.Trim().ToLowerInvariant(),cancellationToken)
                .ConfigureAwait(false);

            String claveHash = String.Empty;

            var usuarios = login.User.Select(item => new DTOs.Auth.UserDto(
                item.IdUsuario,
                item.IdPersonal,
                item.ApePatPer,
                item.ApeMatPer,
                item.NomPer,
                item.TipDocPer,
                item.NumDocPer,
                item.FehNacPer,
                item.FotPer,
                item.IdPerfil,
                item.Perfil,
                item.Correo,
                item.ClaveHash))
            .ToArray();

            var usuario = usuarios[0];

            claveHash = usuario.ClaveHash;

            if (login.User is null || !BCrypt.Net.BCrypt.Verify(request.Clave, claveHash))
            {
                return Response<LoginResponseDTO>.Failure("Credenciales inválidas.", Array.Empty<string>());
            }

            var (token, expiry) = jwtTokenService.GenerateToken(mapper.Map<UserDto>(usuario));

            var menu = login.Menu
            .Select(item => new DTOs.Auth.MenuDto(
                item.IdPerfil,
                item.Perfil,
                item.IdMenu,
                item.Menu,
                item.IdSubMenu,
                item.NomVista,
                item.NomUrl,
                item.Icono,
                item.Tipo,
                item.Cantidad))
            .ToArray();

            var responseData = new LoginResponseDTO(
            token,
            expiry,
            usuarios,
            menu);

            return Response<LoginResponseDTO>.Ok(responseData);

        }
    }
}
