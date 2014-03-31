using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Toolkit.Plugins;

namespace Leon.Plugins
{
    public class HomePlugin:PluginBase
    {
        public override System.Web.Mvc.ActionResult Execute()
        {
            ViewBag.Pluign = "This is from plugin";

            return null;
        }
    }
}
