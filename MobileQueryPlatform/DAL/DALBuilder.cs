using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interface;
using System.Data;

namespace DAL
{
    public class DALBuilder
    {
        /// <summary>
        /// 创建DAL实例
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="DBType">0 MSSql， 1 Oracle</param>
        /// <returns></returns>
        public static IDAL CreateDAL(string connectionString, int DBType)
        {
            switch (DBType)
            {
                case 0:
                    return new MsSQLDAL(connectionString);
                case 1:
                    return new OracleDAL(connectionString);
                default:
                    throw new Exception("未找到该数据库类型");
            }
        }
    }
}
