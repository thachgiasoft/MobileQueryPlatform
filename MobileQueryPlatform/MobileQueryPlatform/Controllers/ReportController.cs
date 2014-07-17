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
        public ResultModel<object> Put(decimal id, Report value)
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
        public ResultModel<object> Delete(decimal id)
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
