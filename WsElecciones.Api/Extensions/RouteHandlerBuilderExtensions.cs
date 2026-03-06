using WsElecciones.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace WsElecciones.Api.Extensions;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder NotRequiredCompania(this RouteHandlerBuilder builder) =>
        builder.WithMetadata(new RequiredCompaniaAttribute(false));
}
