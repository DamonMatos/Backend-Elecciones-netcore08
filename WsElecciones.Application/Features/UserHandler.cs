using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Auth;
using WsElecciones.Application.Enums;
using WsElecciones.CrossCutting;
using WsElecciones.CrossCutting.Storage;
using WsElecciones.Domain;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.Auth;
using UserDto = WsElecciones.Domain.Views.Auth.UserDto;

namespace WsElecciones.Application.Features
{
    public class UserHandler(IMapper mapper, IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IFileStorageService fileStorage, IOptions<FileStorageConfig> storageOptions)// IConfiguration config)
    {
        private static readonly HashSet<string> AllowedPublicRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            UserRoles.Admin,
            "Administrador",
            "Empresa"
        };

        public async Task<Response<LoginResponseDTO>> RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken)
        {
            var role = string.IsNullOrWhiteSpace(request.Perfil) ? "Empresa" : request.Perfil;

            if (!AllowedPublicRoles.Contains(role))
            {
                return Response<LoginResponseDTO>.Failure("Credenciales inválidas.", Array.Empty<string>());
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Clave, workFactor: 12);

            var result = await unitOfWork.AuthRepository.CreateAsync(request.Correo, passwordHash, request.Perfil, cancellationToken);

            var userDto = mapper.Map<ResponseDTO>(result);

            if (userDto.Estado == (int)EstadoResultado.ConError)
            {
                return Response<LoginResponseDTO>.Failure(userDto.Mensaje, Array.Empty<string>());
            }

            var authView = await unitOfWork.AuthRepository.GetByEmailAsync(request.Correo, cancellationToken);

            var usuario = authView.User;

            if (usuario is null)
                return Response<LoginResponseDTO>.Failure("Usuario registrado pero no se pudo iniciar sesión.", Array.Empty<string>());

            var (token, expiry) = jwtTokenService.GenerateToken(usuario);

            var storage = storageOptions.Value.Modules["Personal"];
            var baseUrl = storage.BaseUrl;

            //var baseUrl = config["FileStorage:BaseUrl"]!;
            var fotUrl = string.IsNullOrWhiteSpace(usuario.FotPer) ? $"{baseUrl}/user-default.png" : $"{baseUrl}/{usuario.NumDocPer.Trim()}/{usuario.FotPer}";

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

            var menu = authView.Menu
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
            user,
            menu);

            return Response<LoginResponseDTO>.Ok(responseData);

        }

        public async Task<Response<ResponseDTO>> UpdateAsync(UpdateRequestDTO request, CancellationToken cancellationToken)
        {
            string foldername = string.Empty;
            string modulo = string.Empty;   
            if (request.Foto is not null)
            {
                var extension = Path.GetExtension(request.Foto.FileName).ToLowerInvariant();
                if (extension != ".png")
                {
                    return Response<ResponseDTO>.Failure("Solo se permiten archivos.png", Array.Empty<string>());
                }
                foldername = request.NumeroDocumento;
                modulo = "Personal";
                fileStorage.SaveAsync(modulo, foldername, request.NombreFoto, request.Foto, cancellationToken);
            }

            var result = await unitOfWork.AuthRepository.UpdateAsync(mapper.Map<UpdateUserView>(request), cancellationToken);

            var userDto = mapper.Map<ResponseDTO>(result);

            if (userDto.Estado == (int)EstadoResultado.ConError)
            {
                return Response<ResponseDTO>.Failure(userDto.Mensaje, Array.Empty<string>());
            }

            return Response<ResponseDTO>.Ok(userDto);

        }

    }
}
