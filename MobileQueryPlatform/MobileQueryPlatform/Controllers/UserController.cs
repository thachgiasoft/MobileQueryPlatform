using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;
using BLL;

namespace MobileQueryPlatform.Controllers
{
    public class UserController : ApiController
    {
        public IEnumerable<User> Get()
        {
            return UserBLL.ListUser();
        }

        // GET api/user
        public IEnumerable<User> Get(string UserCode,string UserName,string IsAdmin)
        {
            return UserBLL.ListUser(UserCode,UserName,IsAdmin);
        }

        // GET api/user/5
        public User Get(int id) 
        {
            return null;
        }

        // POST api/user
        public ResultModel<object> Post(User value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            rst.ResultStatus= UserBLL.AddUser(value, out rst.ResultMessage);
            return rst;
        }

        // PUT api/user/5
        public ResultModel<object> Put(int id, User value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            rst.ResultStatus = UserBLL.UpdateUser(value, out rst.ResultMessage);
            return rst;
        }

        // DELETE api/user/5
        public ResultModel<object> Delete(int id)
        {
            ResultModel<object> rst = new ResultModel<object>();
            rst.ResultStatus = UserBLL.DeleteUser(id, out rst.ResultMessage);
            return rst;
        }
    }
}
