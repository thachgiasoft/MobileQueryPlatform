using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobileQueryPlatform.Controllers
{
    public class UserReportController : ApiController
    {
        // GET api/userreport
        public IEnumerable<UserReport> Get(decimal userID)
        {
            return null;
        }

        // POST api/userreport
        public void Post(UserReport value)
        {
        }

        // PUT api/userreport/5
        public void Put(decimal id, UserReport value)
        {
        }

        // DELETE api/userreport/5
        public void Delete(decimal id)
        {
        }
    }
}
