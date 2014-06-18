using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileQueryPlatform.Attribute;
using Model;

namespace MobileQueryPlatform.Controllers
{
    [AuthorizationAttribute]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            User user = Session["SigninedUser"] as User;
            if (!user.IsAdmin)
            {
                return new RedirectResult("/Signin/AdminSignin");
            }
            return View();
        }

        /// <summary>
        /// 菜单
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Menu()
        {
            return PartialView();
        }

        /// <summary>
        /// 数据库管理页面
        /// </summary>
        /// <returns></returns>
        public PartialViewResult DatabaseManage()
        {
            return PartialView();
        }

        /// <summary>
        /// 报表管理页面
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ReportManage()
        {
            return PartialView();
        }

        /// <summary>
        /// 系统参数管理界面
        /// </summary>
        /// <returns></returns>
        public PartialViewResult SystemManage()
        {
            return PartialView();
        }

        /// <summary>
        /// 用户管理界面
        /// </summary>
        /// <returns></returns>
        public PartialViewResult UserManage()
        {
            return PartialView();
        }

    }
}
