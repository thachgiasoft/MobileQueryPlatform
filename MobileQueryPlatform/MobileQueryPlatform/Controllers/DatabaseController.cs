using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobileQueryPlatform.Controllers
{
    public class DatabaseController : ApiController
    {
        // GET api/database
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/database/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/database
        public void Post([FromBody]string value)
        {
        }

        // PUT api/database/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/database/5
        public void Delete(int id)
        {
        }
    }
}
