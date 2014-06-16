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
        // GET api/user
        public IEnumerable<User> Get()
        {
            return UserBLL.ListUser();
        }

        // GET api/user/5
        public User Get(int id)
        {
            return null;
        }

        // POST api/user
        public void Post(User value)
        {
        }

        // PUT api/user/5
        public void Put(int id, User value)
        {
        }

        // DELETE api/user/5
        public void Delete(int id)
        {
        }
    }
}
