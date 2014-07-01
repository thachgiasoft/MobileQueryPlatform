using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileQueryPlatform.Attribute;

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
    }
}
