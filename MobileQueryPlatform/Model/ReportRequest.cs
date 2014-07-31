using System;
using System.ComponentModel;
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
        public ReportParam[] Params
        {
            get;
            set;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortColumn
        {
            get;
            set;
        }

        /// <summary>
        /// 排序方向
        /// </summary>
        [DefaultValue(false)]
        public bool Desc
        {
            get;
            set;
        }
    }
}
