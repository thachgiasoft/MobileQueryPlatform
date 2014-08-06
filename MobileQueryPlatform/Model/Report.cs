using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        public string DBCode
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
        public string SqlCommand
        {
            get;
            set;
        }

        const string ORDERBY_REGEX = @"\sorder\s+by\s";
        /// <summary>
        /// 语句中是否含有OrderBy
        /// </summary>
        public bool CommandHasOrderby
        {
            get
            {
                return string.IsNullOrEmpty(SqlCommand)?false:Regex.IsMatch(SqlCommand, ORDERBY_REGEX);
            }
        }

        /// <summary>
        /// 可排序
        /// </summary>
        public bool Sortabled
        {
            get
            {
                if (Columns == null)
                {
                    return false;
                }
                return !CommandHasOrderby && Columns.Where(c => c.Sortabled).Count() > 0 ;
            }
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
        /// 字符型 = 0,
        /// 数值型 = 1,
        /// 日期型 = 2
        /// </summary>
        public short ColumnType
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

        /// <summary>
        /// 当前排序值
        /// 0 -不排序
        /// 1 -升序
        /// 2 -降序
        /// </summary>
        [DefaultValue(0)]
        public short SortValue
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
        /// 字符型 = 0,
        /// 数值型 = 1,
        /// 日期型 = 2
        /// </summary>
        public short ParamType
        {
            get;
            set;
        }

        public string ParamTypeName
        {
            get
            {
                switch (ParamType)
                {
                    case 0:
                        return "字符型";
                    case 1:
                        return "数值型";
                    case 2:
                        return "日期型";
                    default:
                        return "类型错误";
                }
            }
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

        public string ParamInputTypeName
        {
            get
            {
                switch (ParamInputType)
                {
                    case 0:
                        return "手动输入";
                    case 1:
                        return "列表输入";
                    default:
                        return "输入类型错误";
                }
            }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue
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

        const string REPORT_PARAM_ITEM_REGEX = @"[^\d;]\w*=\w*";
        public string ParamItemString
        {
            get
            {
                if (ParamItems == null || ParamItems.Count == 0)
                {
                    return string.Empty;
                }
                StringBuilder strb=new StringBuilder(256);
                
                    switch (ParamInputType)
                    {
                        case 0:
                            foreach (ReportParamItem item in ParamItems)
                            {
                                strb.AppendFormat("{0}=\"{1}\";", item.OptionName, item.OptionValue);
                            }
                            return strb.ToString();
                        case 1:
                            foreach (ReportParamItem item in ParamItems)
                            {
                                strb.AppendFormat("{0}={1};", item.OptionName, item.OptionValue);
                            }
                            return strb.ToString();
                        default:
                            return string.Empty;
                    }
            }
            set
            {
                ParamItems = new List<ReportParamItem>();
                if (!string.IsNullOrEmpty(value))
                {
                    MatchCollection ms = Regex.Matches(value, REPORT_PARAM_ITEM_REGEX);
                    foreach (Match m in ms)
                    {
                        string[] p = m.Value.Split('=');
                        ParamItems.Add(new ReportParamItem()
                        {
                            ReportID = ReportID,
                            ParamCode = ParamCode,
                            OptionName = p[0],
                            OptionValue = p[1]
                        });
                    }
                }
            }
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
