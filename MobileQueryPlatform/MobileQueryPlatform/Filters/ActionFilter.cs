using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
namespace MobileQueryPlatform.Filters
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple=false)]
    public class ActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 操作前过滤
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string DomainName = filterContext.HttpContext.Request.Url.Authority;
            //string Module = filterContext.HttpContext.Request.Url.Segments[0];
            //UserModel user = filterContext.HttpContext.Session["User"] as UserModel;
            //string ControllerName = filterContext.RouteData.Values["controller"].ToString();
            //string ActionName = filterContext.RouteData.Values["action"].ToString();
            //if (user == null && ControllerName!="Home" && ActionName!="Signin")
            //{
            //    //没有权限,自动跳转到登录页面
            //    ViewResult view = new ViewResult();
            //    view.ViewName = "Index";
            //    filterContext.Result = view;
            //}
        }

        /// <summary>
        /// 操作后执行
        /// 记录日志
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //string DomainName = filterContext.HttpContext.Request.Url.Authority;
            //string Module = filterContext.HttpContext.Request.Url.Segments[0];
            //UserModel user = filterContext.HttpContext.Session["LoginedUser"] as UserModel;
            //string ControllerName = filterContext.RouteData.Values["controller"].ToString();
            //string ActionName = filterContext.RouteData.Values["action"].ToString();
            //if (filterContext.Result.GetType() == typeof(JsonResult))
            //{
            //    //获取返回数据的类型
            //    object Data = ((JsonResult)filterContext.Result).Data;
            //    string DataType = Data.GetType().Name;
            //}
        }
    }
}

