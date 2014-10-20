using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Leon.Modules.IPFilter.Filters
{
    public class IPFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new IPFilter().Do();
            base.OnActionExecuting(filterContext);
        }
    }
}