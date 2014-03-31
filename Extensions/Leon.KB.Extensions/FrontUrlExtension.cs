using System;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Web;

namespace Leon.KB.Extensions
{
    public static class FrontUrlExtension
    {
        public static FrontUrlHelper FrontUrlInLowercase(this UrlHelper url, Site site,
            FrontRequestChannel requestChannel = FrontRequestChannel.Host)
        {
            return new FrontUrlHelperWrapper(url.FrontUrl(site, requestChannel));
        }


        public class FrontUrlHelperWrapper : Kooboo.CMS.Sites.View.FrontUrlHelper
        {
            private FrontUrlHelper _frontUrlHelper;

            public FrontUrlHelperWrapper(FrontUrlHelper frontUrlHelper)
                : base(frontUrlHelper.Url, frontUrlHelper.Site, frontUrlHelper.RequestChannel)
            {
                _frontUrlHelper = frontUrlHelper;
            }

            public override IHtmlString PageUrl(string urlMapKey)
            {
                string pageUrl = _frontUrlHelper.PageUrl(urlMapKey).ToString();
                pageUrl = DealWithUrl(pageUrl);

                return new HtmlString(pageUrl);
            }

            public override IHtmlString PageUrl(string urlMapKey, object values)
            {
                string pageUrl = _frontUrlHelper.PageUrl(urlMapKey, values).ToString();
                pageUrl = DealWithUrl(pageUrl);

                return new HtmlString(pageUrl);
            }

            public override IHtmlString PageUrl(string urlMapKey, object values, out Page page)
            {
                string pageUrl = _frontUrlHelper.PageUrl(urlMapKey, values, out page).ToString();
                pageUrl = DealWithUrl(pageUrl);

                return new HtmlString(pageUrl);
            }

            private string DealWithUrl(string url)
            {
                if (url.Contains("?"))
                {
                    var urlArray = url.Split('?');
                    if (urlArray.Length > 0)
                    {
                        string firstPart = urlArray[0];
                        url = url.Replace(firstPart, firstPart.ToLower());

                        return url;
                    }
                }

                return String.IsNullOrEmpty(url) ? String.Empty : url.ToLower();
            }
        }
    }
}
