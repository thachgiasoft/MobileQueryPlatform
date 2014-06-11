using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 列表参数选项
    /// </summary>
    public class ReportParamOption
    {
        public decimal ID
        {
            get;
            set;
        }

        public decimal ParamID
        {
            get;
            set;
        }

        public decimal OptionIndex
        {
            get;
            set;
        }

        public string OptionText
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
