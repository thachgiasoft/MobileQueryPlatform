using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileQueryPlatform.Attribute;
using Model;
using BLL;

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
            ViewBag.IsAdmin = user.UserCode=="admin";
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

        public JsonResult TestDatabase(string dataSource, string dbName, string userID, string password, short dbType)
        {
            Database db = new Database()
            {
                DataSource = dataSource,
                DbName = dbName,
                UserID = userID,
                Password = password,
                DbType = dbType
            };
            ResultModel<object> rst = new ResultModel<object>();
            rst.ResultStatus = DatabaseBLL.TestDatabase(db, out rst.ResultMessage);
            return Json(rst);
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

        public PartialViewResult UserReport()
        {
            return PartialView();
        }

        public PartialViewResult ChangePassword()
        {
            User user = Session["SigninedUser"] as User;
            ViewBag.UserID = user.ID;
            return PartialView();
        }

        public JsonResult DoChangePassword(string oldPwd, string newPwd, decimal id)
        {
            ResultModel<object> rst = new ResultModel<object>();
            rst.ResultStatus = UserBLL.ChangePwd(id, oldPwd, newPwd, out rst.ResultMessage);
            return Json(rst);
        }
    }
}
