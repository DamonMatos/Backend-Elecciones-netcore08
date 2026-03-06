using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WsElecciones.Domain;
using System.Data.Common;
using System.Xml.Linq;

namespace WsElecciones.Persistence.Context
{
    /// <summary>
    /// Clase base genérica para ejecutar procedimientos almacenados usando ADO.NET.
    /// Implementación inyectable de IStoredProcedureExecutor.
    /// </summary>
    public sealed class StoredProcedureExecutor(DbContext context) : IStoredProcedureExecutor
    {
        private async Task<DbConnection> EnsureOpenConnectionAsync(CancellationToken cancellationToken)
        {
            var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            return connection;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que devuelve un único conjunto de resultados (lectura simple).
        /// </summary>
        public async Task<List<T>> ExecuteReaderAsync<T>(
            string spName,
            Func<IDataRecord, T> mapFunction,
            IEnumerable<SqlParameter>? parameters = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(spName)) throw new ArgumentException("spName is required", nameof(spName));
            if (mapFunction is null) throw new ArgumentNullException(nameof(mapFunction));

            var connection = await EnsureOpenConnectionAsync(cancellationToken);
            await using var command = connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters is not null)
                foreach (var p in parameters) command.Parameters.Add(p);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

            var result = new List<T>();
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                result.Add(mapFunction(reader));
            }

            return result;
        }

        /// <summary>
        /// Ejecuta un SP con múltiples conjuntos de resultados (varios SELECT).
        /// </summary>
        //public async Task<List<List<T>>> ExecuteMultipleReaderAsync<T>(
        //    string spName,
        //    Func<IDataRecord, T> mapFunction,
        //    IEnumerable<SqlParameter>? parameters = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    if (string.IsNullOrWhiteSpace(spName)) throw new ArgumentException("spName is required", nameof(spName));
        //    if (mapFunction is null) throw new ArgumentNullException(nameof(mapFunction));

        //    var connection = await EnsureOpenConnectionAsync(cancellationToken);
        //    await using var command = connection.CreateCommand();
        //    command.CommandText = spName;
        //    command.CommandType = CommandType.StoredProcedure;

        //    if (parameters is not null)
        //        foreach (var p in parameters) command.Parameters.Add(p);

        //    var resultSets = new List<List<T>>();

        //    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        //    do
        //    {
        //        var currentSet = new List<T>();
        //        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        //        {
        //            currentSet.Add(mapFunction(reader));
        //        }

        //        if (currentSet.Count > 0)
        //            resultSets.Add(currentSet);

        //    } while (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false));

        //    return resultSets;
        //}
        public async Task<List<List<object>>> ExecuteMultipleReaderAsync(
            string storedProcedure,
            List<Func<IDataRecord, object>> mapFunction,
            IEnumerable<SqlParameter>? parameters = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("spName is required", nameof(storedProcedure));

            var connection = await EnsureOpenConnectionAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = storedProcedure;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters is not null)
                foreach (var p in parameters) command.Parameters.Add(p);

            var results = new List<List<object>>();
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            int index = 0;

            do
            {
                if (index >= mapFunction.Count)
                    break;

                var mapper = mapFunction[index];
                var list = new List<object>();

                while (await reader.ReadAsync(cancellationToken))
                {
                    list.Add(mapper(reader));
                }
                results.Add(list);
                index++;

            } while (await reader.NextResultAsync(cancellationToken));
            return results;
        }
        /// <summary>
        /// Ejecuta un SP que no devuelve resultados (INSERT/UPDATE/DELETE).
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(
            string spName,
            IEnumerable<SqlParameter>? parameters = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(spName)) throw new ArgumentException("spName is required", nameof(spName));

            var connection = await EnsureOpenConnectionAsync(cancellationToken);
            await using var command = connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters is not null)
                foreach (var p in parameters) command.Parameters.Add(p);

            var rows = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            return rows;
        }

        /// <summary>
        /// Ejecuta un SP que devuelve un único valor escalar (ej: Id generado).
        /// </summary>
        public async Task<T?> ExecuteScalarAsync<T>(
            string spName,
            IEnumerable<SqlParameter>? parameters = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(spName)) throw new ArgumentException("spName is required", nameof(spName));

            var connection = await EnsureOpenConnectionAsync(cancellationToken);
            await using var command = connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters is not null)
                foreach (var p in parameters) command.Parameters.Add(p);

            var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            if (result is null || result == DBNull.Value) return default;

            try
            {
                var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                var converted = Convert.ChangeType(result, targetType);
                return (T?)converted;
            }
            catch
            {
                return default;
            }
        }
    }
}