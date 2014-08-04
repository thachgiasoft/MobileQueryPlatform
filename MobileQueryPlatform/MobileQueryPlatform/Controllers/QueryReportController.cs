using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;

namespace MobileQueryPlatform.Controllers
{
    public class QueryReportController : ApiController
    {
        // POST api/queryreport
        public ResultModel<ReportResult> Post(ReportRequest request)
        {
            ResultModel<ReportResult> rst = new ResultModel<ReportResult>();
            rst.ResultStatus = ReportBLL.QueryReport(request, out rst.ResultObj, out rst.ResultMessage);
            return rst;
        }

    }
}
