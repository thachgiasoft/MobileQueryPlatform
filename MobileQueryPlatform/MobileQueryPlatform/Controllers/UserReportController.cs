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
        public ResultModel<object> Post(UserReport[] collection)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = ReportBLL.SaveUserReport(collection[0].UserID, collection, out rst.ResultMessage);
            return rst;
        }
    }
}
