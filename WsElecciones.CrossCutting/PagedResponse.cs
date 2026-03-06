namespace WsElecciones.CrossCutting;

public sealed record PagedResponse<T>(
    IReadOnlyCollection<T> Items,
    int TotalRegistros,
    int Page,
    int Limit
);
