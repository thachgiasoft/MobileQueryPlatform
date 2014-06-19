using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 数据库模型
    /// </summary>
    public class Database
    {
        public decimal ID
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库编码
        /// </summary>
        public string DBCode
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库类型
        /// 0-MSSql 1-Oracle
        /// </summary>
        public int DBType
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }
    }

}
