using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UserReport
    {
        public decimal ReportID
        {
            get;
            set;
        }

        public decimal UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 报表是否可见
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }

        public string ReportName
        {
            get;
            set;
        }
    }
}
