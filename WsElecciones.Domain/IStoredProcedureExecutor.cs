using Microsoft.Data.SqlClient;
using System.Data;

namespace WsElecciones.Domain
{
    public interface IStoredProcedureExecutor
    {
        Task<List<T>> ExecuteReaderAsync<T>(
            string storedProcedure,
            Func<IDataRecord, T> mapFunction,
            IEnumerable<SqlParameter>? parameters = null,
            CancellationToken cancellationToken = default);

        //Task<List<List<T>>> ExecuteMultipleReaderAsync<T>(
        //    string storedProcedure,
        //    Func<IDataRecord, T> mapFunction,
        //    IEnumerable<SqlParameter>? parameters = null,
        //    CancellationToken cancellationToken = default);

        Task<List<List<object>>> ExecuteMultipleReaderAsync(
            string storedProcedure,
            List<Func<IDataRecord, object>> mapFunction,
            IEnumerable<SqlParameter>? parameters = null,
            CancellationToken cancellationToken = default);

        Task<int> ExecuteNonQueryAsync(
            string storedProcedure,
            IEnumerable<SqlParameter>? parameters = null,
            CancellationToken cancellationToken = default);

        Task<T?> ExecuteScalarAsync<T>(
            string storedProcedure,
            IEnumerable<SqlParameter>? parameters = null,
            CancellationToken cancellationToken = default);
    }
}
