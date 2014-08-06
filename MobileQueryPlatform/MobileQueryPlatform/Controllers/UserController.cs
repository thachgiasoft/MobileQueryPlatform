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

        // POST api/user
        public User Post(User value)
        {
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus= UserBLL.AddUser(ref value, out ResultMessage);
            if (ResultStatus == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            else if (ResultStatus == -1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return value;
        }

        // PUT api/user/5
        public void Put(decimal id, User value)
        {
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = UserBLL.UpdateUser(value, out ResultMessage);
            if (ResultStatus == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            else if (ResultStatus == -1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        // DELETE api/user/5
        public void Delete(decimal id)
        {
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = UserBLL.DeleteUser(id, out ResultMessage);
            if (ResultStatus==0)
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
