using MobileQueryPlatform.Filters;
using System.Web;
using System.Web.Mvc;

namespace MobileQueryPlatform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ActionFilter());
        }
    }
}