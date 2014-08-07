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
        public void Post(Syscfg value)
        {
            User user = HttpContext.Current.Session["SigninedUser"] as User;
            if (user == null || !user.IsAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = SyscfgBLL.SaveCfg(value, out ResultMessage);
            if (ResultStatus == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            else if (ResultStatus == -1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }
    }
}
