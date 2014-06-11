using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enum;

namespace Model
{
    /// <summary>
    /// 报表输入参数
    /// </summary>
    public class ReportParam
    {
        public decimal ID
        {
            get;
            set;
        }
        public decimal ReportID
        {
            get;
            set;
        }

        public string ParamCode
        {
            get;
            set;
        }

        public string ParamName
        {
            get;
            set;
        }

        /// <summary>
        /// 参数类型
        /// </summary>
        public ParamType ParamType
        {
            get;
            set;
        }

        /// <summary>
        /// 参数输入方式
        /// </summary>
        public ParamInputType ParamInputType
        {
            get;
            set;
        }
    }
}
