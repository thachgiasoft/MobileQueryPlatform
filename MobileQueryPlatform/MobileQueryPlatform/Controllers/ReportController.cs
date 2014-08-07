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
    public class ReportController : ApiController
    {
        // GET api/report
        public ICollection<Report> Get()
        {
            return ReportBLL.ListReport();
        }

        // GET api/report/5
        public Report Get(decimal id)
        {
            return ReportBLL.GetReport(id);
        }

        public Report Get(decimal id, bool forQuery)
        {
            return ReportBLL.GetReportForQuery(id);
        }
        /// <summary>
        /// 重构报表结构
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Report Get(decimal id, decimal DBID, string sql)
        {
            string msg;
            return ReportBLL.RebuildReport(id, DBID,sql, out msg);
        }

        // POST api/report
        public Report Post(Report value)
        {
            User user = HttpContext.Current.Session["SigninedUser"] as User;
            if (user == null || !user.IsAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = ReportBLL.InsertReport(ref value, out ResultMessage);
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

        // PUT api/report/5
        public void Put(decimal id, Report value)
        {
            User user = HttpContext.Current.Session["SigninedUser"] as User;
            if (user == null || !user.IsAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = ReportBLL.UpdateReport(id, value, out ResultMessage);
            if (ResultStatus == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            else if (ResultStatus == -1)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        // DELETE api/report/5
        public void Delete(decimal id)
        {
            User user = HttpContext.Current.Session["SigninedUser"] as User;
            if (user == null || !user.IsAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = ReportBLL.DeleteReport(id, out ResultMessage);
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
