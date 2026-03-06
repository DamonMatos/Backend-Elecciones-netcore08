using WsElecciones.CrossCutting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog.Context;
using System.Net;
using System.Text.Json;

namespace WsElecciones.Api.Middleware
{
    public class ExceptionMiddleware
    {
        public const string CodUsuarioItemKey = "CodUsuario";
        private const string DefaultCodUsuario = "USERCTACTE";
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private static ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var traceidentifier = Guid.NewGuid().ToString();
            Constants.TraceIdentifier = traceidentifier;

            try
            {
                var endpoint = httpContext.GetEndpoint();
                if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
                {
                    await _next(httpContext);
                    return;
                }

                httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationfromheader);

                var token = authorizationfromheader.Count > 0 ? authorizationfromheader[0] : string.Empty;
                token = "ABC";
                if (!string.IsNullOrEmpty(token))
                {
                    Constants.CodUsuario = httpContext.Items[CodUsuarioItemKey]?.ToString() ?? DefaultCodUsuario;
                    //token = token?.Split(" ").Last();
                    //Constants.CodUsuario = "";//JwtHelper.ValidateToken(token, _configuration["JwtConfig:Secret"] ?? throw new ArgumentNullException("Not Passing JWT secret")); // aqui deberia leer el endpoint para JWT
                    Constants.JWT = token;
                    await EnrichLogWithHttpContext(httpContext);
                    await _next.Invoke(httpContext);
                }
                else
                {
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync("Unauthorized for this request.");
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                await HandleTokenExpiredException(httpContext, traceidentifier, ex);
            }
            /*catch (BussinessLogicException ex)
            {
                await HandleBusinessLogicException(httpContext, ex, traceidentifier);
            }*/
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }

        }

        /*private static async Task HandleBusinessLogicException(HttpContext context, BussinessLogicException ex, string traceindetifier)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;

            await context.Response.WriteAsync(ex.Message);
        }*/

        private static async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            _logger.LogError(ex, "({@AppName} -> [{@Username}] {@TraceIdentifier} - {@Message} | {@QueryString} | {@RequestBody} | {@RouteParams}",
                Constants.AppName,
                Constants.CodUsuario,
                Constants.TraceIdentifier,
                ex.Message,
                Constants.QueryString,
                Constants.RequestBody,
                Constants.RouteParams);
            var message = Constants.IsInDevelopment ? ex.Message : string.Format(Message.UnexpectedErrorMessage, Constants.TraceIdentifier);
            await context.Response.WriteAsJsonAsync(Response<object?>.Failure(message, []));
            //await context.Response.WriteAsync(message);
        }

        private static async Task HandleTokenExpiredException(HttpContext context, string traceidentifier, SecurityTokenExpiredException ex)
        {

            context.Response.StatusCode = StatusCodes.Status511NetworkAuthenticationRequired;

            _logger.LogCritical(ex, "({@AppName} -> [{@Username}] {@TraceIdentifier} - Token is expired for {@Username}",
                Constants.AppName,
                Constants.CodUsuario,
                traceidentifier,
                Constants.CodUsuario);

            await context.Response.WriteAsync("El token expiró");
        }

        private static async Task EnrichLogWithHttpContext(HttpContext httpContext)
        {
            LogContext.PushProperty("AppName", Constants.AppName);
            LogContext.PushProperty("Traceidentifier", Constants.TraceIdentifier);
            LogContext.PushProperty("Username", Constants.CodUsuario);

            if (httpContext.Request.QueryString.HasValue)
            {
                Constants.QueryString = httpContext.Request.QueryString.Value;
            }

            if (httpContext.Request.RouteValues.Any())
            {
                var routeParameters = new List<string>();
                foreach (var routeParam in httpContext.Request.RouteValues)
                {
                    routeParameters.Add($"{routeParam.Key}:{routeParam.Value?.ToString()}");
                }

                Constants.RouteParams = JsonSerializer.Serialize(routeParameters);
            }

            if (httpContext.Request.Method == HttpMethods.Post || httpContext.Request.Method == HttpMethods.Put)
            {
                httpContext.Request.EnableBuffering();

                var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();

                httpContext.Request.Body.Position = 0;

                if (!string.IsNullOrEmpty(requestBody))
                {
                    Constants.RequestBody = requestBody;
                }
            }
        }

    }
}
