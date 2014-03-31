#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Search;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.Extensions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Site = Kooboo.CMS.Sites.Models.Site;
using Page = Kooboo.CMS.Sites.Models.Page;

namespace Leon.KB.Extensions.Pages
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IObjectConverter), Key = "Kooboo.CMS.Sites.Models.Page", Order = 100)]
    public class PageConverter : IObjectConverter
    {
        public KeyValuePair<string, string> GetKeyField(object o)
        {
            Page page = (Page)o;
            return new KeyValuePair<string, string>("PageName", page.FullName);
        }

        public IndexObject GetIndexObject(object o)
        {
            IndexObject indexObject = null;

            var page = (Page)o;
            var storeFields = new NameValueCollection();
            var sysFields = new NameValueCollection();

            sysFields.Add("SiteName", page.Site.FullName);
            sysFields.Add("PageName", page.FullName);

            string title = "";
            var body = new StringBuilder();

            if (page.HtmlMeta != null && !string.IsNullOrEmpty(page.HtmlMeta.HtmlTitle))
            {
                title = page.HtmlMeta.HtmlTitle;
            }
            if (!string.IsNullOrEmpty(page.ContentTitle))
            {
                body.Append(title);

                title = page.ContentTitle;
            }

            if (page.PagePositions != null)
            {
                foreach (var item in page.PagePositions.Where(it => (it is HtmlBlockPosition) || (it is HtmlPosition)))
                {
                    if (item is HtmlBlockPosition)
                    {
                        var htmlBlockPosition = (HtmlBlockPosition)item;
                        var htmlBlock = new HtmlBlock(page.Site, htmlBlockPosition.BlockName).LastVersion().AsActual();
                        if (htmlBlock != null)
                        {
                            body.Append(" " + Kooboo.StringExtensions.StripAllTags(htmlBlock.Body));
                        }
                    }
                    else
                    {
                        var htmlPosition = (HtmlPosition)item;
                        body.Append(" " + Kooboo.StringExtensions.StripAllTags(htmlPosition.Html));
                    }
                }
            }


            indexObject = new IndexObject()
            {
                Title = title,
                Body = body.ToString(),
                StoreFields = storeFields,
                SystemFields = sysFields,
                NativeType = typeof(Page).AssemblyQualifiedNameWithoutVersion()
            };

            return indexObject;
        }

        public static Site CurrentSite { get; set; }

        public object GetNativeObject(System.Collections.Specialized.NameValueCollection fields)
        {
            var siteName = fields["SiteName"];
            var pageName = fields["PageName"];
            return new Page(Page_Context.Current.PageRequestContext.Site, pageName);
        }

        public string GetUrl(object nativeObject)
        {
            Page page = (Page)nativeObject;

            if (CurrentSite != null && CurrentSite.Name != Page_Context.Current.FrontUrl.Site.AsActual().Name)
            {
                return new UrlHelper(HttpContext.Current.Request.RequestContext).FrontUrl(CurrentSite, Page_Context.Current.PageRequestContext.RequestChannel).PageUrl(page.FullName).ToString();
            }
            else
            {
                return Page_Context.Current.FrontUrl.PageUrl(page.FullName).ToString();
            }
        }
    }
}
