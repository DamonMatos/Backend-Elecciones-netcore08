using Microsoft.Data.SqlClient;
using System.Data;

namespace WsElecciones.Persistence.SqlHelpers
{
    public static class SqlParameterFactory
    {
        public static SqlParameter CreateNullableVarchar(string name, int size, string? value) =>
            new(name, SqlDbType.VarChar, size) { Value = value ?? (object)DBNull.Value };

        public static SqlParameter CreateVarchar(string name, int size, string value) =>
            new(name, SqlDbType.VarChar, size) { Value = value };

        public static SqlParameter CreateInt(string name, int value) =>
            new(name, SqlDbType.Int) { Value = value };

        public static SqlParameter CreateNullableInt(string name, int? value) =>
            new(name, SqlDbType.Int) { Value = value ?? (object)DBNull.Value };

        public static SqlParameter CreateBigInt(string name, long value) =>
            new(name, SqlDbType.BigInt) { Value = value };

        public static SqlParameter CreateNullableBigInt(string name, long? value) =>
            new(name, SqlDbType.BigInt) { Value = value ?? (object)DBNull.Value };

        public static SqlParameter CreateDecimal(string name, decimal value) =>
            new(name, SqlDbType.Decimal) { Value = value };

        public static SqlParameter CreateNullableDecimal(string name, decimal? value) =>
            new(name, SqlDbType.Decimal) { Value = value ?? (object)DBNull.Value };

        public static SqlParameter CreateMoney(string name, decimal value) =>
            new(name, SqlDbType.Money) { Value = value };

        public static SqlParameter CreateNullableMoney(string name, decimal? value) =>
            new(name, SqlDbType.Money) { Value = value ?? (object)DBNull.Value };

        public static SqlParameter CreateDateTime(string name, DateTime value) =>
            new(name, SqlDbType.DateTime) { Value = value };

        public static SqlParameter CreateDateTime(string name, DateTime? value) =>
            new(name, SqlDbType.DateTime)
            {
                Value = value ?? (object)DBNull.Value
            };

        public static SqlParameter CreateNullableDateTime(string name, DateTime? value) =>
            new(name, SqlDbType.DateTime) { Value = value ?? (object)DBNull.Value };

        public static SqlParameter CreateNullableBit(string name, bool? value) =>
            new(name, SqlDbType.Bit) { Value = value ?? (object)DBNull.Value };

        public static SqlParameter CreateChar(string name, int size, char value) =>
            new(name, SqlDbType.Char, size) { Value = value };

        public static SqlParameter CreateNullableChar(string name, int size, char? value) =>
            new(name, SqlDbType.Char, size) { Value = value ?? (object)DBNull.Value };

        public static SqlParameter CreateOutput(string name, SqlDbType type, int? size = null, object? value=null)
        {
            var param = size.HasValue
                ? new SqlParameter(name, type, size.Value)
                : new SqlParameter(name, type);

            param.Direction = ParameterDirection.Output;
            param.Value = value;
            return param;
        }
        public static SqlParameter CreateStructured(string name, string typeName, DataTable value) =>
            new(name, value) { SqlDbType = SqlDbType.Structured, TypeName = typeName };

       
    }
}
