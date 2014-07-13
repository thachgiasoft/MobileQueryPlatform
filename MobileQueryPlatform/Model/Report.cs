using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 报表标题
    /// 用于列表显示
    /// </summary>
    public class Report
    {
        #region ReportHead
        /// <summary>
        /// ID
        /// </summary>
        public decimal ID
        {
            get;
            set;
        }


        /// <summary>
        /// 数据库ID
        /// </summary>
        public decimal DBID
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DBName
        {
            get;
            set;
        }

        /// <summary>
        /// 报表名称
        /// </summary>
        public string ReportName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 字段集合
        /// </summary>
        public ICollection<ReportColumn> Columns
        {
            get;
            set;
        }

        /// <summary>
        /// 参数集合
        /// </summary>
        public ICollection<ReportParam> Params
        {
            get;
            set;
        }

        /// <summary>
        /// 报表指令
        /// </summary>
        public ReportCommand Command
        {
            get;
            set;
        }

        /// <summary>
        /// 报表结果
        /// </summary>
        public ReportResult Result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 报表体
    /// 报表主要配置内容
    /// </summary>
    public class ReportResult
    {
        public decimal ReportID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用页合计
        /// </summary>
        public bool PageSumabled
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用总合计
        /// </summary>
        public bool AllSumabled
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用分页
        /// </summary>
        public bool Pagingabled
        {
            get;
            set;
        }

        /// <summary>
        /// 每页数据条数
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }
    }

    public class ReportCommand
    {

        public decimal ReportID
        {
            get;
            set;
        }

        public string SqlCommand
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 字段名
    /// </summary>
    public class ReportColumn
    {
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
        /// 是否允许排序
        /// </summary>
        public bool Sortabled
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 报表输入参数
    /// </summary>
    public class ReportParam
    {
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
        /// 文本类型 = 0,
        /// 数值类型 = 1,
        /// 日期类型 = 2
        /// </summary>
        public short ParamType
        {
            get;
            set;
        }

        /// <summary>
        /// 参数输入方式
        /// 手动输入 = 0,
        /// 列表输入 = 1
        /// </summary>
        public short ParamInputType
        {
            get;
            set;
        }

        /// <summary>
        /// 列表参数集合
        /// </summary>
        public ICollection<ReportParamItem> ParamItems
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 列表参数选项
    /// </summary>
    public class ReportParamItem
    {
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

        public string OptionName
        {
            get;
            set;
        }

        public string OptionValue
        {
            get;
            set;
        }
    }

}
