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
    public class UserReportController : ApiController
    {
        // GET api/userreport
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="model">0-仅本用户菜单，1-全部菜单</param>
        /// <returns></returns>
        public IEnumerable<UserReport> Get(decimal userID,short model)
        {
            switch (model)
            {
                case 0:
                    return ReportBLL.ListUserReport(userID);
                case 1:
                    return ReportBLL.ListAllUserReport(userID);
                default:
                    return null;
            }
        }

        // POST api/userreport
        public void Post(UserReport[] collection)
        {
            User user = HttpContext.Current.Session["SigninedUser"] as User;
            if (user == null || !user.IsAdmin)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            string ResultMessage;
            int ResultStatus = ReportBLL.SaveUserReport(collection[0].UserID, collection, out ResultMessage);
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
