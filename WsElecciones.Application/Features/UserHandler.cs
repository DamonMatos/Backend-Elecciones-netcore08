using MapsterMapper;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Auth;
using WsElecciones.Application.Enums;
using WsElecciones.CrossCutting;
using WsElecciones.Domain;
using WsElecciones.Domain.Entities;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.Auth;

namespace WsElecciones.Application.Features
{
    public class UserHandler(IMapper mapper, IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IFileStorageService fileStorage)
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

            var result = await unitOfWork.AuthRepository.RegistrarUserAsysc(request.Correo, passwordHash, request.Perfil, cancellationToken);

            var userDto = mapper.Map<IReadOnlyList<ResponseDTO>>(result);

            if (userDto[0].Estado == (int)EstadoResultado.ConError)
            {
                return Response<LoginResponseDTO>.Failure(userDto[0].Mensaje, Array.Empty<string>());
            }

            var authView = await unitOfWork.AuthRepository.GetByUsernameAsync(request.Correo, cancellationToken);

            var usuario = authView.User.SingleOrDefault();

            if (usuario is null)
                return Response<LoginResponseDTO>.Failure("Usuario registrado pero no se pudo iniciar sesión.", Array.Empty<string>());

            var (token, expiry) = jwtTokenService.GenerateToken(usuario);

            var usuarios = authView.User.Select(item => new DTOs.Auth.UserDto(
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
            usuarios,
            menu);

            return Response<LoginResponseDTO>.Ok(responseData);

        }

        public async Task<Response<ResponseDTO>> UpdateAsync(UpdateRequestDTO request, CancellationToken cancellationToken)
        {
            string foldername = string.Empty;

            if (request.Foto is not null)
            {
                var extension = Path.GetExtension(request.Foto.FileName).ToLowerInvariant();
                if (extension != ".png")
                {
                    return Response<ResponseDTO>.Failure("Solo se permiten archivos.png", Array.Empty<string>());
                }
                foldername = "Personal";

                fileStorage.SaveAsync(request.Foto, foldername, request.NombreFoto, cancellationToken);
            }

            var result = await unitOfWork.AuthRepository.UpdateUserAsysc(mapper.Map<UpdateUserView>(request), cancellationToken);

            var userDto = mapper.Map<IReadOnlyList<ResponseDTO>>(result);

            if (userDto[0].Estado == (int)EstadoResultado.ConError)
            {
                return Response<ResponseDTO>.Failure(userDto[0].Mensaje, Array.Empty<string>());
            }

            return Response<ResponseDTO>.Ok(userDto[0]);

        }

    }
}
