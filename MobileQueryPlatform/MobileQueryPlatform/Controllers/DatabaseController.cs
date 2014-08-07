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
        public Database Get(decimal ID)
        {
            return DatabaseBLL.GetDatabase(ID);
        }
        // GET api/database
        public IEnumerable<Database> Get()
        {
            return DatabaseBLL.ListDatabase();
        }

        
        // POST api/database
        public Database Post(Database value)
        {
            User user = HttpContext.Current.Session["SigninedUser"] as User;
            if (user == null || !user.IsAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = DatabaseBLL.InsertDatabase(ref value, out ResultMessage);
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

        // PUT api/database/5
        public void Put(decimal id, Database value)
        {
            User user = HttpContext.Current.Session["SigninedUser"] as User;
            if (user == null || !user.IsAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = DatabaseBLL.UpdateDatabase(id, value, out ResultMessage);
            if (ResultStatus == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            else if (ResultStatus == -1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        // DELETE api/database/5
        public void Delete(decimal id)
        {
            User user = HttpContext.Current.Session["SigninedUser"] as User;
            if (user == null || !user.IsAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = DatabaseBLL.DeleteDatabase(id, out ResultMessage);
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
