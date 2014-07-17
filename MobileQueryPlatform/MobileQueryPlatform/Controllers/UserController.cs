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
    public class UserController : ApiController
    {
        public IEnumerable<User> Get()
        {
            return UserBLL.ListUser();
        }

        //// GET api/user
        //public IEnumerable<User> Get(string UserCode,string UserName,string IsAdmin)
        //{
        //    return UserBLL.ListUser(UserCode,UserName,IsAdmin);
        //}

        // POST api/user
        public ResultModel<object> Post(User value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus= UserBLL.AddUser(value, out rst.ResultMessage);
            return rst;
        }

        // PUT api/user/5
        public ResultModel<object> Put(decimal id, User value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = UserBLL.UpdateUser(value, out rst.ResultMessage);
            return rst;
        }

        // DELETE api/user/5
        public ResultModel<object> Delete(decimal id)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = UserBLL.DeleteUser(id, out rst.ResultMessage);
            return rst;
        }
    }
}
