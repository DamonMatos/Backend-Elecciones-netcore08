using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using WsElecciones.CrossCutting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WsElecciones.Api.Middleware;

public sealed class HttpRequestMiddleware
{
    public const string CodCompaniaItemKey = "CodCompania";

    private readonly RequestDelegate _next;
    private readonly ILogger<HttpRequestMiddleware> _logger;

    public HttpRequestMiddleware(RequestDelegate next, ILogger<HttpRequestMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        var endpoint = context.GetEndpoint();
        if (endpoint is null)
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        var requirements = endpoint.Metadata.GetOrderedMetadata<RequireHttpRequestValueAttribute>();
        var requiredCompania = requirements.FirstOrDefault(x=> x.ItemKey == CodCompaniaItemKey) ?? new RequiredCompaniaAttribute();
        //Constants.CodCompania = "1";
        if (requiredCompania.Required)
        {
            Constants.CodCompania = context.Request.Headers[requiredCompania.ItemKey];
            if (Constants.CodCompania is null)
            {
                JsonDocument? requestBodyDocument = null;
                try
                {
                    var value = ExtractFromHeader(context, requiredCompania.HeaderName);

                    if (string.IsNullOrWhiteSpace(value) &&
                        !string.IsNullOrWhiteSpace(requiredCompania.BodyPropertyName))
                    {
                        requestBodyDocument = await TryReadRequestBodyAsync(context).ConfigureAwait(false);

                        if (requestBodyDocument?.RootElement.ValueKind == JsonValueKind.Object &&
                            requestBodyDocument.RootElement.TryGetProperty(requiredCompania.BodyPropertyName, out var propertyElement) &&
                            propertyElement.ValueKind == JsonValueKind.String)
                        {
                            Constants.CodCompania = propertyElement.GetString();
                        }
                    }
                }
                catch
                {
                    await WriteMissingValueResponse(context, new RequiredCompaniaAttribute()).ConfigureAwait(false);
                    return;
                }
                finally
                {
                    requestBodyDocument?.Dispose();
                }
            }
            if (Constants.CodCompania is null)
            {
                await WriteMissingValueResponse(context, new RequiredCompaniaAttribute()).ConfigureAwait(false);
                return;
            }
        }
        /* Por ahora solo validaremos compañia
        var requestBodyReadAttempted = false;
        try
        {
            foreach (var requirement in requirements)
            {
                if (context.Items.ContainsKey(requirement.ItemKey))
                {
                    continue;
                }

                var value = ExtractFromHeader(context, requirement.HeaderName);

                if (string.IsNullOrWhiteSpace(value) &&
                    !string.IsNullOrWhiteSpace(requirement.BodyPropertyName))
                {
                    if (!requestBodyReadAttempted)
                    {
                        requestBodyDocument = await TryReadRequestBodyAsync(context).ConfigureAwait(false);
                        requestBodyReadAttempted = true;
                    }

                    if (requestBodyDocument?.RootElement.ValueKind == JsonValueKind.Object &&
                        requestBodyDocument.RootElement.TryGetProperty(requirement.BodyPropertyName, out var propertyElement) &&
                        propertyElement.ValueKind == JsonValueKind.String)
                    {
                        value = propertyElement.GetString();
                    }
                }

                value = string.IsNullOrWhiteSpace(value) ? requirement.DefaultValue : value;
                value = string.IsNullOrWhiteSpace(value) ? null : value.Trim();

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (requirement.Required)
                    {
                        await WriteMissingValueResponse(context, requirement).ConfigureAwait(false);
                        return;
                    }

                    continue;
                }
                context.Items[requirement.ItemKey] = value;
            }
        }
        finally
        {
            requestBodyDocument?.Dispose();
        }*/

        await _next(context).ConfigureAwait(false);
    }

    private static string? ExtractFromHeader(HttpContext context, string? headerName)
    {
        if (string.IsNullOrWhiteSpace(headerName) ||
            !context.Request.Headers.TryGetValue(headerName, out var values))
        {
            return null;
        }

        var headerValue = values.FirstOrDefault();
        return string.IsNullOrWhiteSpace(headerValue) ? null : headerValue;
    }

    private async Task<JsonDocument?> TryReadRequestBodyAsync(HttpContext context)
    {
        if (!HttpMethods.IsPost(context.Request.Method) &&
            !HttpMethods.IsPut(context.Request.Method) &&
            !HttpMethods.IsPatch(context.Request.Method))
        {
            return null;
        }

        if (!context.Request.ContentLength.HasValue || context.Request.ContentLength.Value == 0)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(context.Request.ContentType) ||
            !context.Request.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        context.Request.EnableBuffering();

        string body;

        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
        {
            body = await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        context.Request.Body.Position = 0;

        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        try
        {
            return JsonDocument.Parse(body);
        }
        catch (JsonException exception)
        {
            _logger.LogWarning(exception, "No se pudo leer el cuerpo de la petición.");
        }

        return null;
    }

    private Task WriteMissingValueResponse(HttpContext context, RequireHttpRequestValueAttribute requirement)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var message = $"El campo {requirement.DisplayName} es obligatorio.";
        var details = new[]
        {
            $"Debe enviarse el {requirement.DisplayName} en la petición."
        };

        return context.Response.WriteAsJsonAsync(Response<object?>.Failure(message, details));
    }
}
