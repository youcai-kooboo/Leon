using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;

using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;

namespace Kooboo.CMS.Toolkit.Modules
{
    public class ModuleMenuItemInitializer : DefaultMenuItemInitializer
    {
        protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
        {
            string moduleRepositoryName = controllerContext.RequestContext.GetRequestValue("moduleRepositoryName");
            string moduleRepositoryName2 = controllerContext.RequestContext.GetRequestValue("moduleRepositoryName2");

            string folderName = controllerContext.RequestContext.GetRequestValue("folderName");

            if (!String.IsNullOrWhiteSpace(moduleRepositoryName))
            {
                return menuItem.RouteValues.GetString("moduleRepositoryName").Equals(moduleRepositoryName, StringComparison.OrdinalIgnoreCase) && menuItem.RouteValues.GetString("folderName").Equals(folderName, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return menuItem.RouteValues.GetString("moduleRepositoryName2").Equals(moduleRepositoryName2, StringComparison.OrdinalIgnoreCase) && menuItem.RouteValues.GetString("folderName").Equals(folderName, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}