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

    }
}
