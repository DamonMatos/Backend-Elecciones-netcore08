using WsElecciones.Api.Endpoints.Enums;
using WsElecciones.Api.Endpoints.Options;
using WsElecciones.CrossCutting;
using WsElecciones.Api.Validations;
using FluentValidation;

namespace WsElecciones.Api.Extensions
{
    public static class EndpointExtensions
    {
        public static RouteHandlerBuilder MapEndpoint<TResponse>(
            this RouteGroupBuilder group,
            HttpMethodType method,
            string pattern,
            string name,
            Delegate handler,
            EndpointOptions? options = null)
        {
            var builder = method switch
            {
                HttpMethodType.Get => group.MapGet(pattern, handler),
                HttpMethodType.Post => group.MapPost(pattern, handler),
                HttpMethodType.Put => group.MapPut(pattern, handler),
                HttpMethodType.Delete => group.MapDelete(pattern, handler),
                _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
            };

            builder.WithName(name);

            if (options?.NotRequiredCompania == true)
                builder.NotRequiredCompania();

            if (options?.RequireAuthorization == true)
                builder.RequireAuthorization();

            if(options?.RequireValidation == true)
            {
                var request = handler.Method.GetParameters().FirstOrDefault(p =>
                !typeof(HttpContext).IsAssignableFrom(p.ParameterType) &&
                !typeof(CancellationToken).IsAssignableFrom(p.ParameterType));
                if(request is not null)
                {
                    var type = request.ParameterType;
                    if(type.IsClass && type != typeof(string) && !type.IsPrimitive && !type.IsEnum)
                    {
                        var filterType = typeof(ValidationFilter<,>).MakeGenericType(type, typeof(TResponse));
                        var filterInstance = Activator.CreateInstance(filterType) as IEndpointFilter;
                        if (filterInstance is not null)
                        {
                            builder.AddEndpointFilter(filterInstance);
                        }
                    }
                }
            }
            builder.AddStandardProduces<TResponse>(method);

            return builder;
        }



        /// <summary>  vmatos
        /// Requiere token JWT válido + uno o más roles permitidos.
        /// Uso: .RequireTokenAndRole("Admin")
        /// Uso: .RequireTokenAndRole("Admin", "Administrador")
        /// </summary>
        public static RouteHandlerBuilder RequireTokenAndRole(
            this RouteHandlerBuilder builder,
            params string[] roles)
        {
            return builder.RequireAuthorization(policy =>
            {
                policy.RequireAuthenticatedUser(); // token válido y no expirado
                policy.RequireRole(roles);         // perfil permitido
            });
        }


        private static void AddStandardProduces<TResponse>(this RouteHandlerBuilder builder, HttpMethodType method)
        {
            var successCodes = method switch
            {
                HttpMethodType.Get => [HttpStatusCode.Ok, HttpStatusCode.NotFound],
                HttpMethodType.Post => [HttpStatusCode.Created],
                HttpMethodType.Put => [HttpStatusCode.Ok],
                HttpMethodType.Delete => new[] { HttpStatusCode.Ok },
                _ => []
            };

            var errorCodes = new[]
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.InternalServerError
            };

            foreach (var code in successCodes)
            {
                if (code == HttpStatusCode.NoContent)
                    builder.Produces(code);
                else
                    builder.Produces<Response<TResponse>>(code);
            }

            foreach (var code in errorCodes)
            {
                builder.Produces<Response<string>>(code);
            }
        }
    }
    public static class HttpStatusCode
    {
        public const int Ok = StatusCodes.Status200OK;
        public const int Created = StatusCodes.Status201Created;
        public const int NoContent = StatusCodes.Status204NoContent;
        public const int NotFound = StatusCodes.Status404NotFound;
        public const int BadRequest = StatusCodes.Status400BadRequest;
        public const int Unauthorized = StatusCodes.Status401Unauthorized;
        public const int InternalServerError = StatusCodes.Status500InternalServerError;
    }
}
