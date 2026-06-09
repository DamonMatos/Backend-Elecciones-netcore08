using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WsElecciones.Application.DTOs.Auth;
using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Storage;
using WsElecciones.Domain;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.Auth;
using static System.Runtime.InteropServices.JavaScript.JSType;
using UserDto = WsElecciones.Domain.Views.Auth.UserDto;

namespace WsElecciones.Application.Features
{
    public class LoginHandler(IMapper mapper,IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IOptions<FileStorageConfig> storageOptions)//, IConfiguration config)
    {
        public async Task<Response<LoginResponseDTO>> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken= default)
        {
            var login = await unitOfWork.AuthRepository.GetByEmailAsync(request.Correo.Trim().ToLowerInvariant(), cancellationToken)
                .ConfigureAwait(false);

            var usuario = login.User;

            if (usuario is null)
                return Response<LoginResponseDTO>.Failure("Credenciales inválidas.", Array.Empty<string>());

            if (!BCrypt.Net.BCrypt.Verify(request.Clave, usuario.ClaveHash))
                return Response<LoginResponseDTO>.Failure("Credenciales inválidas.",Array.Empty<string>());

            var storage = storageOptions.Value.Modules["Personal"];
            var baseUrl = storage.BaseUrl;

            //var baseUrl = config["FileStorage:BaseUrl"]!;
            var fotUrl = string.IsNullOrWhiteSpace(usuario.FotPer)? $"{baseUrl}/user-default.png": $"{baseUrl}/{usuario.NumDocPer.Trim()}/{usuario.FotPer}";

            var user = new DTOs.Auth.UserDto(
                usuario.IdUsuario,
                usuario.IdPersonal,
                usuario.ApePatPer,
                usuario.ApeMatPer,
                usuario.NomPer,
                usuario.TipDocPer,
                usuario.NumDocPer,
                usuario.FehNacPer,
                fotUrl,
                usuario.IdPerfil,
                usuario.Perfil,
                usuario.Correo,
                string.Empty
            );

            var (token, expiry) = jwtTokenService.GenerateToken(mapper.Map<UserDto>(user));

            var menu = mapper.Map<IReadOnlyCollection<DTOs.Auth.MenuDto>>(login.Menu);

            var response = new LoginResponseDTO(
            token,
            expiry,
            user,
            menu);

            return Response<LoginResponseDTO>.Ok(response);
        }
    }
}
