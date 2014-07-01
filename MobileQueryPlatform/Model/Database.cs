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
        public string DbCode
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库类型
        /// 0-MSSql 1-Oracle
        /// </summary>
        public int DbType
        {
            get;
            set;
        }

        public string DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库 或 服务名
        /// </summary>
        public string DbName
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

    }

}
