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
        public Report Get(int id ,bool forEdit)
        {
            return ReportBLL.GetReport(id, forEdit);
        }

        // POST api/report
        public ResultModel<object> Post(Report value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = ReportBLL.InsertReport(value, out rst.ResultMessage);
            return rst;
        }

        // PUT api/report/5
        public ResultModel<object> Put(int id, Report value)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = ReportBLL.UpdateReport(id, value, out rst.ResultMessage);
            return rst;
        }

        // DELETE api/report/5
        public ResultModel<object> Delete(int id)
        {
            ResultModel<object> rst = new ResultModel<object>();
            if (HttpContext.Current.Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return rst;
            }
            rst.ResultStatus = ReportBLL.DeleteReport(id, out rst.ResultMessage);
            return rst;
        }
    }
}
