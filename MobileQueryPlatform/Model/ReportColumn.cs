using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 字段名
    /// </summary>
    public class ReportColumn
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

        public string ColumnCode
        {
            get;
            set;
        }

        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用求和
        /// </summary>
        public bool Sumabled
        {
            get;
            set;
        }

        /// <summary>
        /// 字段列宽
        /// 有效值1-12
        /// </summary>
        public decimal ColumnWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许排序
        /// </summary>
        public bool Sortabled
        {
            get;
            set;
        }
    }
}
