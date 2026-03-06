
namespace WsElecciones.Domain.Views.Elecciones;

public sealed record EleccionesView(
    int IdEleccion,
    int IdCliente,
    string RazonSocial,
    string RUC,
    string Nombre,
    string ColorBase,
    string UrlLogo,
    string NumDocPer,
    string FechaDifusion,
    string FechaInicio,
    string FechaFin,
    string FechaRegistro,
    bool PlanillaConfirmada,
    bool DifusionEnviada,
    int Estado
);

public sealed record EleccionesPagedResult(
    IReadOnlyCollection<EleccionesView> Items,
    int TotalRegistros,
    int Page,
    int Limit) : PagedView <EleccionesView>(Items, TotalRegistros, Page, Limit);
