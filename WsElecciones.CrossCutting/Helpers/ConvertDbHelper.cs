using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsElecciones.CrossCutting.Helpers
{
    public static class ConvertDbHelper
    {
        public static DateTime? ToDateTimeNull(object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == "null" || obj.ToString() == null) return null; else return Convert.ToDateTime(obj);
        }
        public static DateTime ToDateTime(object obj)
        {
            if (obj == null || obj == DBNull.Value || obj.ToString() == "null" || obj.ToString() == null) return DateTime.MinValue; else return Convert.ToDateTime(obj);
        }
        public static int ToInt32(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? 0 : Convert.ToInt32(obj.ToString());
        }

        public static int? ToInt32Null(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? (int?)null : Convert.ToInt32(obj.ToString());
        }

        public static long ToLong(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? 0 : Convert.ToInt64(obj.ToString());
        }

        public static long? ToLongNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? (long?)null : Convert.ToInt64(obj.ToString());
        }

        public static double? ToDoubleNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? (double?)null : Convert.ToDouble(obj.ToString());
        }

        public static double ToDouble(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? 0 : Convert.ToDouble(obj.ToString());
        }

        public static decimal? ToDecimalNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? (decimal?)null : Convert.ToDecimal(obj.ToString());
        }

        public static decimal ToDecimal(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? 0 : Convert.ToDecimal(obj.ToString());
        }

        public static short? ToShortNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? (short?)null : Convert.ToInt16(obj.ToString());
        }
        public static string ToString(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? null : obj.ToString();
        }
        public static bool? ToBooleanNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? (bool?)null : Convert.ToBoolean(obj.ToString());
        }

        public static bool ToBoolean(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || obj.ToString() == "null" || obj.ToString() == null) ? false : Convert.ToBoolean(obj.ToString());
        }
    }
}
