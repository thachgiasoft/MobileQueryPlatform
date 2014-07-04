using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;

namespace MobileQueryPlatform.Controllers
{
    public class ReportRequestController : Controller
    {
        //
        // GET: /ReportRequest/

        /// <summary>
        /// 请求报表数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Request(decimal id,ReportRequest request)
        {
            ResultModel<string> rst=new ResultModel<string>();
            if (Session["SigninedUser"] == null)
            {
                rst.ResultMessage = "用户登录失效";
                rst.ResultStatus = -1;
                return Json(rst);
            }
            rst.ResultStatus = ReportBLL.QueryReport(id, request,out rst.ResultObj,out rst.ResultMessage);
            return Json(rst);
        }

    }
}
