using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ResultModel<T>
    {
        /// <summary>
        /// 结果状态
        /// 0-正常失败 1-成功 -1 -异常
        /// </summary>
        public int ResultStatus;

        /// <summary>
        /// 结果对象
        /// </summary>
        public T ResultObj;

        /// <summary>
        /// 结果消息
        /// </summary>
        public string ResultMessage;
    }
}
