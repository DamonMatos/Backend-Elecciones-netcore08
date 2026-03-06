namespace WsElecciones.Api.Middleware;

public sealed class RequiredCompaniaAttribute : RequireHttpRequestValueAttribute
{
    public RequiredCompaniaAttribute(bool required = true)
        : base(
            HttpRequestMiddleware.CodCompaniaItemKey,
            required: required,
            headerName: "codCompania",
            bodyPropertyName: "codCompania",
            displayName: "codCompania")
    {
    }
}
