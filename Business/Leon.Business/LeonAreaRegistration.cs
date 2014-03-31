using System.IO;
using System.Web.Mvc;
using Kooboo;
using Leon.Business.Controllers;

namespace Leon.Business
{
    public class LeonAreaRegistration : AreaRegistration
    {
        public const string Area = "_Leon";

        public override string AreaName
        {
            get
            {
                return Area;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "_Leon_default",
                "_Leon/{controller}/{action}/{UserKey}",
                new { action = "Index", UserKey = UrlParameter.Optional },
                new[] { typeof(SyncController).Namespace, "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );

            //Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "Menu.config"));
            //Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config"));
        }
    }
}
