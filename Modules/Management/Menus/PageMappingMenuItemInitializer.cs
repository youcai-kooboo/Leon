using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Sites.Models;

namespace Leon.Modules.Management.Menus
{
    public class PageMappingMenuItemInitializer : Kooboo.CMS.Toolkit.Modules.ModuleMenuItemInitializer
    {
        protected override bool GetIsVisible(MenuItem menuItem, ControllerContext controllerContext)
        {
            // Visible in Root site
            Site site = Site.Current;
            if(site != null)
            {
                if(site.Parent != null)
                {
                    return false;
                }
            }

            return base.GetIsVisible(menuItem, controllerContext);
        }
    }
}