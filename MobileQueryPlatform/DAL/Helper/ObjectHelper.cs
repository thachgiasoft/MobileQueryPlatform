using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DAL.Helper
{
    public class ObjectHelper
    {
        #region 从数据集对象生成对象

        /// <summary>
        /// 组装对象
        /// </summary>
        /// <typeparam name="T">对象泛型</typeparam>
        /// <param name="dt">数据源DataTable</param>
        /// <returns>返回的对象集合</returns>
        public static ICollection<T> BuildObject<T>(DataTable dt)
        {
            ICollection<T> objs = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T t = BuildObject<T>(row);
                objs.Add(t);
            }
            return objs;
        }

        /// <summary>
        /// 组装 DataObject
        /// </summary>
        /// <typeparam name="T">对象泛型</typeparam>
        /// <param name="dataReader">数据源DataReader</param>
        /// <returns>返回的对象集合</returns>
        public static ICollection<T> BuildObject<T>(IDataReader dataReader)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Load(dataReader);
                return BuildObject<T>(dt);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 组装DataObject
        /// </summary>
        /// <typeparam name="T">对象泛型</typeparam>
        /// <param name="row">数据源DataRow</param>
        /// <returns>返回的对象</returns>
        public static T BuildObject<T>(DataRow row)
        {
            T obj = Activator.CreateInstance<T>();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo p in propertyInfos)
            {
                switch (p.PropertyType.Name)
                {
                    case "String":
                        p.SetValue(obj, Convert.ToString(row[p.Name]).TrimEnd(), null);
                        break;
                    case "Int32":
                        p.SetValue(obj, Convert.ToInt32(row[p.Name]), null);
                        break;
                    case "Decimal":
                        p.SetValue(obj, Convert.ToDecimal(row[p.Name]), null);
                        break;
                    case "DateTime":
                        p.SetValue(obj, Convert.ToDateTime(row[p.Name]), null);
                        break;
                    case "Single":
                        p.SetValue(obj, Convert.ToSingle(row[p.Name]), null);
                        break;
                    case "Boolean":
                        p.SetValue(obj, Convert.ToInt16(row[p.Name])==1?true:false, null);
                        break;
                }
            }
            return obj;
        }

        #endregion
    }
}
