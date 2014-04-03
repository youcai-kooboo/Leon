using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.ModuleArea.Monitor.Filters;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.ModuleArea.Monitor.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Repositories;

namespace Kooboo.CMS.ModuleArea.Monitor
{
    [Dependency(typeof(IHttpApplicationEvents), Key = "MonitorHttpApplicationEvents")]
    public class MonitorHttpApplicationEvents : HttpApplicationEvents
    {
        private IEnumerable<string> _siteModules;
        private IEnumerable<string> SiteModules
        {
            get
            {
                if (_siteModules == null)
                {
                    _siteModules = Kooboo.CMS.Sites.Services.ServiceFactory.ModuleManager.AllModulesForSite(Site.Current.FullName);
                }
                return _siteModules;
            }
        }

        public override void Init(HttpApplication httpApplication)
        {
            base.Init(httpApplication);
            System.Web.Mvc.GlobalFilters.Filters.Add(new ErrorFilterAttribute());

            httpApplication.ReleaseRequestState += (sender, e) =>
            {
                HttpContext.Current.Items[Keys.HTTPCONTEXT_IS_KOOBOO_PAGE] = Page_Context.Current.Initialized;
            };
        }

        public override void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items[Keys.HTTPCONTEXT_BEGINREQUEST_DATETIME] = DateTime.UtcNow;
            base.Application_BeginRequest(sender, e);
        }

        public override void Application_EndRequest(object sender, EventArgs e)
        {
            var isPage = HttpContext.Current.Items[Keys.HTTPCONTEXT_IS_KOOBOO_PAGE] as bool?;
            if (isPage.HasValue&&isPage.Value && SiteModules.Contains(MonitorAreaRegistration.ModuleName,StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    var request = HttpContext.Current.Request;
                    var response = HttpContext.Current.Response;
                    var referer = request.UrlReferrer;
                    var currentDomain = request.Url.Host;
                    HttpCookie cookie = this.GetTraceCookie();
                    string uniqueVisitorKey = GetUniqueVisitorKey(cookie);
                    if (!request.UserAgent.Equals(Keys.KBM_USER_AGENT, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(uniqueVisitorKey))
                    {
                        uniqueVisitorKey = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(12);
                        this.SetUniqueVisitorKey(cookie, uniqueVisitorKey);
                    }
                    DateTime begin=Convert.ToDateTime(HttpContext.Current.Items[Keys.HTTPCONTEXT_BEGINREQUEST_DATETIME]);
                    var log = new Log()
                    {
                        UserAgent = request.UserAgent,
                        UriStem = request.Url.AbsolutePath,
                        UriQuery = request.Url.Query,
                        ClientIP = request.UserHostAddress,
                        Method = request.HttpMethod,
                        Port = request.Url.Port,
                        UserName = HttpContext.Current.User.Identity.Name,
                        SiteName = Site.Current.FullName,
                        Status = response.StatusCode,
                        SubStatus = response.SubStatusCode,
                        TimeTaken = DateTime.UtcNow.Subtract(begin).TotalSeconds,
                        UniqueVisitorKey = uniqueVisitorKey,
                        VisitCount = this.GetVisitCount(cookie) + 1,
                        LastVisitDate = this.GetLastVisitDate(cookie),
                    };
                    if (referer != null && !referer.Host.Equals(currentDomain, StringComparison.OrdinalIgnoreCase))
                    {
                        log.Referrer = referer.AbsoluteUri;
                        log.ReferrerDomain = referer.Host;
                        log.SearchKey = SpiderHelper.ExtractSearchKey(referer);
                    }
                    var ex = HttpContext.Current.Items[Keys.HTTPCONTEXT_EXCEPTION] as Exception;
                    if (ex != null)
                    {
                        log.Exception = ex.Message;
                        log.StackTrace = ex.StackTrace;
                    }
                    LogService.Push(log);
                    this.AddVisitCount(cookie);
                    this.SetLastVisitDate(cookie, DateTime.UtcNow);
                    cookie.Expires = DateTime.UtcNow.AddYears(1);
                    response.Cookies.Set(cookie);
                }
                catch (Exception ex)
                {
                    Kooboo.HealthMonitoring.Log.LogException(ex);
                }
            }
            base.Application_EndRequest(sender, e);
        }

        private HttpCookie GetTraceCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Keys.COOKIE_NAME_FOR_TRACE);
            if (cookie == null)
            {
                cookie = new HttpCookie(Keys.COOKIE_NAME_FOR_TRACE);
            }
            return cookie;
        }

        private int GetVisitCount(HttpCookie cookie)
        {
            int result = 0;
            string str = cookie.Values.Get(Keys.COOKIE_FOR_VISIT_COUNT);
            int.TryParse(str, out result);
            return result;
        }

        private int AddVisitCount(HttpCookie cookie)
        {
            int result = 0;
            string str = cookie.Values.Get(Keys.COOKIE_FOR_VISIT_COUNT);
            int.TryParse(str, out result);
            cookie.Values.Set(Keys.COOKIE_FOR_VISIT_COUNT, (++result).ToString());
            return result;
        }

        private DateTime? GetLastVisitDate(HttpCookie cookie)
        {
            DateTime? result = null;
            string str = cookie.Values.Get(Keys.COOKIE_FOR_LAST_VISIT_DATE);
            DateTime temp = DateTime.MinValue;
            if (!string.IsNullOrEmpty(str) && DateTime.TryParse(str, out temp))
            {
                result = temp;
            }
            return result;
        }

        private void SetLastVisitDate(HttpCookie cookie,DateTime date)
        {
            cookie.Values.Set(Keys.COOKIE_FOR_LAST_VISIT_DATE, date.ToUniversalTime().ToString());
        }

        private string GetUniqueVisitorKey(HttpCookie cookie)
        {
            return cookie.Values.Get(Keys.COOKIE_FOR_UNIQUE_VISITOR_KEY);
        }

        private void SetUniqueVisitorKey(HttpCookie cookie,string key)
        {
            cookie.Values.Set(Keys.COOKIE_FOR_UNIQUE_VISITOR_KEY, key);
        }
    }
}