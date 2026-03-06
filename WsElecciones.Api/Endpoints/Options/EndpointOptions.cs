namespace WsElecciones.Api.Endpoints.Options
{
    public class EndpointOptions
    {
        public bool NotRequiredCompania { get; set; }
        public bool RequireAuthorization { get; set; }
        public bool IncludeDocs { get; set; } = true;
        public bool RequireValidation { get; set; }
    }
}
