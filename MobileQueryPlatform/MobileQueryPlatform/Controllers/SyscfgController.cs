using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;
using BLL;
using System.Web;

namespace MobileQueryPlatform.Controllers
{
    public class SyscfgController : ApiController
    {
        // GET api/syscfg/5
        public Syscfg Get()
        {
            string msg;
            return SyscfgBLL.LoadCfg(out msg);
        }
        // post api/syscfg/5
        public ResultModel<object> Post(Syscfg value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = SyscfgBLL.SaveCfg(value, out rst.ResultMessage);
            return rst;
        }
    }
}
