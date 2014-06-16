using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using BLL;
using Model;

namespace MobileQueryPlatform.Controllers
{
    public class SignController : Controller
    {
        //
        // GET: /Home/

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult UserSignin()
        {
            ViewBag.Action = "doUserSignin";
            ViewBag.Title = "用户登录";
            ViewBag.Success = "/Home";
            return View("Signin");
        }

        public ViewResult AdminSignin()
        {
            ViewBag.Action = "doAdminSignin";
            ViewBag.Title = "管理员登录";
            ViewBag.Success = "/Admin";
            return View("Signin");
        }
        /// <summary>
        /// 普通用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult doUserSignin(string UserCode, string UPassword)
        {
            ResultModel<User> rst = new ResultModel<User>();
            rst.ResultStatus = UserBLL.UserSignin(UserCode, UPassword, out rst.ResultObj, out rst.ResultMessage);
            if (rst.ResultStatus == 1)
            {
                Session["SigninedUser"] = rst.ResultObj;
            }
            return Json(rst);
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult doAdminSignin(string UserCode, string UPassword)
        {
            ResultModel<User> rst = new ResultModel<User>();
            rst.ResultStatus = UserBLL.AdminSignin(UserCode, UPassword,out rst.ResultObj,out rst.ResultMessage);
            if (rst.ResultStatus == 1)
            {
                Session["SigninedUser"] = rst.ResultObj;
            }
            return Json(rst);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public RedirectResult Signoff()
        {
            Session.Clear();
            return Redirect("/Sign/UserSignin");
        }
    }
}
