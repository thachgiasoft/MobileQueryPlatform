using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace DAL.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// 需要加引号类型列表
        /// </summary>
        private static List<Type> NeedQuoteTypeList = new List<Type>() 
        { 
            typeof(string),//字符型
            typeof(bool),//布尔型
            typeof(DateTime)//日期型
        };
        /// <summary>
        /// Dataset转换为Json
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DatasetToJson(DataSet ds)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            foreach (DataTable dt in ds.Tables)
            {
                int i = ds.Tables.IndexOf(dt);
                jsonBuilder.AppendFormat("\"{0}\":{1}",
                    string.IsNullOrEmpty(dt.TableName) ? "tb" + i.ToString() : dt.TableName,
                    DatatableToJson(dt));
                if (i < ds.Tables.Count - 1)
                {
                    jsonBuilder.Append(",");
                }
            }
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// Datatable转换为Json
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DatatableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            foreach (DataRow row in dt.Rows)
            {
                jsonBuilder.Append("{");
                foreach (DataColumn col in dt.Columns)
                {
                    jsonBuilder.AppendFormat("\"{0}\":{1}{2}{1}",
                        col.ColumnName,
                        NeedQuoteTypeList.Contains(col.DataType) ? "\"" : string.Empty,
                        row[col].ToString());
                    if (dt.Columns.IndexOf(col) < dt.Columns.Count - 1)
                    {
                        jsonBuilder.Append(",");
                    }
                }
                jsonBuilder.Append("}");
                if (dt.Rows.IndexOf(row) < dt.Rows.Count - 1)
                {
                    jsonBuilder.Append(",");
                }
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// 参数集合转换为Json
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        public static string ParametersToJson(params IDataParameter[] paramArray)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            for (int i = 0; i < paramArray.Length - 1; i++)
            {
                jsonBuilder.AppendFormat("\"{0}\":{1}{2}{1},",
                    paramArray[i].ParameterName,
                    NeedQuoteTypeList.Contains(DbTypeToType(paramArray[i].DbType)) ? "\"" : string.Empty, paramArray[i].Value.ToString());
            }
            jsonBuilder.AppendFormat("\"{0}\":{1}{2}{1}}",
                    paramArray[paramArray.Length - 1].ParameterName,
                    NeedQuoteTypeList.Contains(DbTypeToType(paramArray[paramArray.Length - 1].DbType)) ? "\"" : string.Empty, paramArray[paramArray.Length - 1].Value.ToString());
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// Type to DbType
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        static DbType TypeToDbType(Type t)
        {
            DbType dbt;
            try
            {
                dbt = (DbType)Enum.Parse(typeof(DbType), t.Name);
            }
            catch
            {
                dbt = DbType.Object;
            }
            return dbt;
        }

        /// <summary>
        /// DbType to Type
        /// create by xuehuibo 2014-03-21
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        static Type DbTypeToType(DbType dbType)
        {
            Type toReturn = typeof(DBNull);

            switch (dbType)
            {
                case DbType.UInt64:
                    toReturn = typeof(UInt64);
                    break;

                case DbType.Int64:
                    toReturn = typeof(Int64);
                    break;

                case DbType.Int32:
                    toReturn = typeof(Int32);
                    break;

                case DbType.UInt32:
                    toReturn = typeof(UInt32);
                    break;

                case DbType.Single:
                    toReturn = typeof(float);
                    break;

                case DbType.Date:
                case DbType.DateTime:
                case DbType.Time:
                    toReturn = typeof(DateTime);
                    break;

                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                    toReturn = typeof(string);
                    break;

                case DbType.UInt16:
                    toReturn = typeof(UInt16);
                    break;

                case DbType.Int16:
                    toReturn = typeof(Int16);
                    break;

                case DbType.SByte:
                    toReturn = typeof(byte);
                    break;

                case DbType.Object:
                    toReturn = typeof(object);
                    break;

                case DbType.VarNumeric:
                case DbType.Decimal:
                    toReturn = typeof(decimal);
                    break;

                case DbType.Currency:
                    toReturn = typeof(double);
                    break;

                case DbType.Binary:
                    toReturn = typeof(byte[]);
                    break;

                case DbType.Double:
                    toReturn = typeof(Double);
                    break;

                case DbType.Guid:
                    toReturn = typeof(Guid);
                    break;

                case DbType.Boolean:
                    toReturn = typeof(bool);
                    break;
            }

            return toReturn;
        }
    }
}