using System;
using System.Collections.Generic;
using System.Linq;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.TopToolbar;
using Kooboo.CMS.Sites.Models;

namespace Leon.Business.ToobarProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IToolbarProvider), Key = "ManagementToolbarProvider")]
    public class ManagementToolbarProvider : IToolbarProvider
    {
        public MvcRoute[] ApplyTo
        {
            get
            {
                return new[]
                {
                    new MvcRoute()
                    {
                        Area = "Contents",
                        Controller = "TextContent",
                        Action = "Index",
                        RouteValues = new Dictionary<string, object>(){{"moduleRepositoryName","Management"}}
                    }
                };
            }
        }
 
        public IEnumerable<ToolbarGroup> GetGroups(System.Web.Routing.RequestContext requestContext)
        {
            return new ToolbarGroup[]
            {
                new ToolbarGroup()
                {
                    GroupName = "Sync",
                    DisplayText = "Sync datas",
                    IconClass = "export",
                    HtmlAttributes = new Dictionary<string, object>()
                }
            };
        }

        private ToolbarButton GetToolbarButton(string groupName, string area, string controller, string action, string commandText, bool needToCheck = false)
        {
            var htmlAttrDics = new Dictionary<string, object>() { { "data-command-type", "AjaxPost" } };
            if (needToCheck)
            {
                htmlAttrDics.Add("data-show-on-check", "Any");
            }

            return new ToolbarButton()
            {
                GroupName = groupName,
                CommandTarget = new MvcRoute() { Action = action, Controller = controller, Area = area },
                CommandText = commandText,
                HtmlAttributes = htmlAttrDics
            };
        }

        public IEnumerable<ToolbarButton> GetButtons(System.Web.Routing.RequestContext requestContext)
        {
            var buttons = new List<ToolbarButton>();

            var siteName = requestContext.HttpContext.Request.QueryString["siteName"];
            var currentSite = SiteHelper.Parse(siteName);
            if (currentSite != null)
            {
                var parentSite = currentSite.Parent.AsActual();
                if (parentSite != null)
                {
                    buttons.Add(GetToolbarButton("Sync", LeonAreaRegistration.Area, "Sync", "SyncFromParentSite", "From Parent Site"));
                }

                var childrenSites = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.ChildSites(currentSite);
                if (childrenSites.Any())
                {
                    buttons.Add(GetToolbarButton("Sync", LeonAreaRegistration.Area, "Sync", "SyncToChildrenSites", "To Children Sites Recursively", true));
                }
            }

            return buttons;
        }

    }
}
