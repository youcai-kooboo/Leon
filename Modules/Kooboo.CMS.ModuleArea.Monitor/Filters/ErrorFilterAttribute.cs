using Kooboo.CMS.Sites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Repositories;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.ModuleArea.Monitor.Filters
{
    public class ErrorFilterAttribute:System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            filterContext.HttpContext.Items[Keys.HTTPCONTEXT_EXCEPTION] = filterContext.Exception;
            base.OnException(filterContext);
        }
    }
}