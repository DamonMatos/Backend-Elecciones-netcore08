namespace WsElecciones.CrossCutting;
using System.Collections.Generic;

public sealed record Response<T>(
    bool Success,
    string Message,
    T? Data,
    IReadOnlyCollection<string> Errors)
{
    public static Response<T> Ok(T data, string message = "Operación completada correctamente") =>
        new(true, message, data, Array.Empty<string>());

    public static Response<T> Failure(string message, IReadOnlyCollection<string> errors) =>
        new(false, message, default, errors);
}