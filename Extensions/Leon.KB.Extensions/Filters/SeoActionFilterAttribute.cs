using System;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Kooboo.CMS.Sites.Controllers;

namespace Leon.KB.Extensions.Filters
{
    public class SeoActionFilterAttribute : ActionFilterAttribute
    {
        private static readonly Regex UpperCaseExpression = new Regex("[A-Z]", RegexOptions.Compiled);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string rawPath = filterContext.RequestContext.HttpContext.Request.Path;
            string path = rawPath;
            string query = filterContext.RequestContext.HttpContext.Request.Url.Query;
            if(!path.Equals("/"))
            {
                if(UpperCaseExpression.IsMatch(path))
                {
                    path = path.ToLower();
                }

                if(String.IsNullOrEmpty(query))
                {
                    if(path.EndsWith("/"))
                    {
                        path = path.TrimEnd('/');
                    }
                }
            }

            bool needRedirect = !path.Equals(rawPath);
            string url = path + query;

            if (needRedirect && filterContext.RequestContext.HttpContext.Request.HttpMethod == "GET" && !url.Contains("dev~"))
            {
                filterContext.Result = new Redirect301Result(url);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}