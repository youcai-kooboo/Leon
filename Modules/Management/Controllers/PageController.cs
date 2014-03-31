using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Services;

namespace Leon.Modules.Management.Controllers
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Web.Areas.Sites.Controllers.PageController),Order=100)]
    public class PageController : Kooboo.CMS.Web.Areas.Sites.Controllers.PageController
    {
        public PageController(IPageProvider pageProvider, PageManager manager, PageCachingManager pageCachingManager)
            : base(pageProvider, manager, pageCachingManager)
        { }

        public override ActionResult Create(Page model)
        {
            ViewResult viewResult = base.Create(model) as ViewResult;

            var page = viewResult.Model as Page;
            if (page.HtmlMeta == null)
            {
                page.HtmlMeta = new HtmlMeta();
            }

            var metaSetting = ManagementContext.Current.MetaSettingService.GetMetaSettingByLayout(page.Layout);
            if (metaSetting != null)
            {
                page.HtmlMeta.HtmlTitle = metaSetting.Title;
                page.HtmlMeta.Description = metaSetting.Description;
            }

            return viewResult;
        }
    }
}