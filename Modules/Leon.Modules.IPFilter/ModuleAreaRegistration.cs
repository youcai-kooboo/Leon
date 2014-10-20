#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using System.Web.Mvc;
using System.IO;
using Kooboo;
using Kooboo.Web.Mvc;
using Leon.Modules.IPFilter.Filters;

namespace Leon.Modules.IPFilter
{
    public class ModuleAreaRegistration : AreaRegistration
    {
        public const string ModuleName = "IPFilter";
        public override string AreaName
        {
            get
            {
                return ModuleName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            GlobalFilters.Filters.Add(new IPFilterAttribute(),0);

            context.MapRoute(
               ModuleName + "_default",
                ModuleName + "/{controller}/{action}",
                new { controller = "Admin", action = "Index" }
                , null
                , new[] { "Leon.Modules.IPFilter.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );
            var areaPath = AreaHelpers.CombineAreaFilePhysicalPath(AreaName);
            if (Directory.Exists(areaPath))
            {
                var menuFile = AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "CMSMenu.config");
                if (File.Exists(menuFile))
                {
                    Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, menuFile);
                }
                var resourceFile = Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config");
                if (File.Exists(resourceFile))
                {
                    Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, resourceFile);
                }
            }
        }
 
    }
}
