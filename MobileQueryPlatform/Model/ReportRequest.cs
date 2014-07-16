using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 报表请求类
    /// </summary>
    public class ReportRequest
    {
        /// <summary>
        /// 报表ID
        /// </summary>
        public decimal ReportID
        {
            get;
            set;
        }



        /// <summary>
        /// 页码
        /// </summary>
        public int Page
        {
            get;
            set;
        }

        /// <summary>
        /// 参数列表
        /// </summary>
        public ICollection<ReportParam> Params
        {
            get;
            set;
        }
    }
}
