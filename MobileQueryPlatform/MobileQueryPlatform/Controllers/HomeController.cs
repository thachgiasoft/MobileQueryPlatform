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
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult About()
        {
            return PartialView();
        }

        public PartialViewResult ReportMenu()
        {
            User user = Session["SigninedUser"] as User;
            ViewBag.UserID = user.ID;
            return PartialView();
        }

        public PartialViewResult ReportView(short id)
        {
            ViewBag.ReportID = id;
            return PartialView();
        }

        public RedirectResult ChangePassword()
        {
            return Redirect("/Admin/ChangePassword");
        }

        public RedirectResult DoChangePassword(string oldPwd, string newPwd, decimal id)
        {
            return Redirect("/Admin/DoChangePassword");
        }
    }
}
