using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;
using System.Web;

namespace MobileQueryPlatform.Controllers
{
    public class DatabaseController : ApiController
    {
        // GET api/database
        public IEnumerable<Database> Get()
        {
            return DatabaseBLL.ListDatabase();
        }

        
        // POST api/database
        public ResultModel<object> Post(Database value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = DatabaseBLL.InsertDatabase(value, out rst.ResultMessage);
            return rst;
        }

        // PUT api/database/5
        public ResultModel<object> Put(int id, Database value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = DatabaseBLL.UpdateDatabase(id, value, out rst.ResultMessage);
            return rst;
        }

        // DELETE api/database/5
        public ResultModel<object> Delete(int id)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = DatabaseBLL.DeleteDatabase(id, out rst.ResultMessage);
            return rst;
        }
    }
}
