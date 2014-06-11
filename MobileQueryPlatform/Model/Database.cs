using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enum;

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
        /// </summary>
        public DBType DBType
        {
            get;
            set;
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库实例或服务名
        /// </summary>
        public string InitialCatalog
        {
            get;
            set;
        }

        /// <summary>
        /// 登录模式
        /// </summary>
        public PersistSecurityInfo PersistSecurityInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal UserID
        {
            get;
            set;
        }

        public string Password
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
