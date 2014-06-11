using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enum;

namespace Model
{
    public class Report
    {
        public decimal ID
        {
            get;
            set;
        }

        public decimal DBID
        {
            get;
            set;
        }

        public string ReportName
        {
            get;
            set;
        }

        public string SqlCommand
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用页合计
        /// </summary>
        public PageSum PageSum
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用总合计
        /// </summary>
        public TotalSum TotalSum
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用分页
        /// </summary>
        public Paging Paging
        {
            get;
            set;
        }

        /// <summary>
        /// 每页数据条数
        /// </summary>
        public decimal PageSize
        {
            get;
            set;
        }
    }
}
