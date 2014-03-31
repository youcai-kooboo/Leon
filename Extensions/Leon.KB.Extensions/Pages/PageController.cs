using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Leon.KB.Extensions.Filters;

namespace Leon.KB.Extensions.Pages
{
    public class PageController : Kooboo.CMS.Sites.Controllers.PageController
    {
        [SeoActionFilter(Order = 0)]
        public override ActionResult Entry()
        {
            PageContext.FrontUrl = new FrontUrlExtension.FrontUrlHelperWrapper(PageContext.FrontUrl);

            return base.Entry();
        }
    }
}
