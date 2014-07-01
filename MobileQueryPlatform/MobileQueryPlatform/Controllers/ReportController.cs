using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobileQueryPlatform.Controllers
{
    public class ReportController : ApiController
    {
        // GET api/report
        public IEnumerable<Report> Get()
        {
            return null;
        }

        // GET api/report/5
        public Report Get(int id)
        {
            return null;
        }

        // POST api/report
        public ResultModel<object> Post([FromBody]string value)
        {
        }

        // PUT api/report/5
        public ResultModel<object> Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/report/5
        public ResultModel<object> Delete(int id)
        {
        }
    }
}
