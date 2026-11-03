using Microsoft.AspNetCore.Mvc;
using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.Api.Extensions;
using WsElecciones.Application.DTOs;
using WsElecciones.Application.DTOs.Auth;
using WsElecciones.Application.DTOs.Cliente;
using WsElecciones.Application.Features;

namespace WsElecciones.Api.Endpoints
{
    public static class AuthEndpoint
    {
        public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/auth").WithTags("Auth");

            group.MapEndpoint<LoginResponseDTO>(
               HttpMethodType.Post,
               "login",
               "Login",
               async (
                   [FromBody] LoginRequestDTO request,
                   LoginHandler handler,
                   CancellationToken cancellationToken) =>
               {
                   var response = await handler.LoginAsync(request, cancellationToken).ConfigureAwait(false);

                   if (!response.Success)
                       return Results.Unauthorized();

                   return Results.Ok(response);
               },new EndpointOptions { RequireValidation = false , NotRequiredCompania = true  }
               
            );

            group.MapEndpoint<LoginResponseDTO>(
                HttpMethodType.Post,
                "register",
                "CrearUsuario",
                async (
                    [FromBody] RegisterRequestDTO request,
                    UserHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.RegisterAsync(request, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                    {
                        return Results.BadRequest(response);
                    }
                    return Results.Ok(response);
                }, new EndpointOptions { RequireValidation = false, NotRequiredCompania = true }
            );

            return group;
        }

        public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/users").WithTags("Users").DisableAntiforgery(); 

            group.MapEndpoint<ResponseDTO>(
                HttpMethodType.Put,
                "update",
                "ActualizarUsuario",
                async (
                    [FromForm] UpdateRequestDTO request,
                    UserHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.UpdateAsync(request, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                    {
                        return Results.BadRequest(response);
                    }
                    return Results.Ok(response);
                },
                new EndpointOptions { RequireValidation = true, NotRequiredCompania = true }

            ).Accepts<UpdateRequestDTO>("multipart/form-data").RequireTokenAndRole("Administrador","Empresa");

            return group;
        }

        public static RouteGroupBuilder MapClienteEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/cliente").WithTags("Cliente").RequireAuthorization();

            group.MapEndpoint<ClienteDto>(
                HttpMethodType.Get,
                "{id:int}",
                "ObtenerClienteId",
                async (
                    int id,
                    ClienteHandler handler,
                    CancellationToken cancellationToken) =>
                {
                    var response = await handler.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

                    if (!response.Success)
                    {
                        return Results.NotFound(response);
                    }
                    return Results.Ok(response);
                }, new EndpointOptions { RequireValidation = false, NotRequiredCompania = true }
            );
            return group;
        }
    }
}
