namespace WsElecciones.Domain.Views
{
    public record PagedView<T>(
        IReadOnlyCollection<T> Items,
        int TotalRegistros,
        int Page,
        int Limit
    );
}
