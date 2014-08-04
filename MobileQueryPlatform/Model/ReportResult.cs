using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ReportResult
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalCount
        {
            get;
            set;
        }

        public int TotalPage
        {
            get {
                int temp = TotalCount / PageSize;
                if (TotalCount % PageSize != 0)
                {
                    temp++;
                }
                return temp;
            }
        }

        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurPage
        {
            get;
            set;
        }
        /// <summary>
        /// 查询结果
        /// </summary>
        public string ReportData
        {
            get;
            set;
        }


    }
}
