using System;

namespace WsElecciones.Api.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequireHttpRequestValueAttribute : Attribute
{
    public RequireHttpRequestValueAttribute(
        string itemKey,
        string? headerName = null,
        string? bodyPropertyName = null,
        string? defaultValue = null,
        bool required = true,
        string? displayName = null)
    {
        ItemKey = itemKey;
        HeaderName = headerName;
        BodyPropertyName = bodyPropertyName;
        DefaultValue = defaultValue;
        Required = required;
        DisplayName = displayName ?? headerName ?? bodyPropertyName ?? itemKey;
    }

    public string ItemKey { get; }
    public string? HeaderName { get; }
    public string? BodyPropertyName { get; }
    public string? DefaultValue { get; }
    public bool Required { get; }
    public string DisplayName { get; }
}
