using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor
{
    public class MonitorMenuItemInitializer : Kooboo.Web.Mvc.Menu.DefaultMenuItemInitializer
    {
        protected override bool GetIsActive(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            return string.Compare(controllerContext.RouteData.Values["controller"].ToString(), menuItem.Controller, true) == 0&&
                string.Compare(controllerContext.RouteData.Values["action"].ToString(), menuItem.Action, true) == 0;
        }
    }
}
