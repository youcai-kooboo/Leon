using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Toolkit;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;

namespace Leon.Modules.IPFilter
{
    public class ModuleMenuItemInitializer : DefaultMenuItemInitializer
    {
        protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
        {
            string moduleName = controllerContext.RequestContext.GetRequestValue("moduleName");
            string folderName = controllerContext.RequestContext.GetRequestValue("folderName");

            return menuItem.RouteValues.GetString("moduleName").Equals(moduleName, StringComparison.OrdinalIgnoreCase) &&
                   menuItem.RouteValues.GetString("folderName").Equals(folderName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
