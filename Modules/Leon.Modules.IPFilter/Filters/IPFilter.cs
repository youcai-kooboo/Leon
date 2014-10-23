using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Leon.Modules.IPFilter.Models;

namespace Leon.Modules.IPFilter.Filters
{
    public class IPFilter
    {
        private  IPFilterContext _filterContext;
        private  Site _currentSite;
        private  IPSetting _ipSetting;

        public IPFilter()
        {
            _currentSite = Site.Current;
            if (_currentSite == null)
            {
                _currentSite = Site.Current = Providers.SiteProvider.AllRootSites().FirstOrDefault();
                if (_currentSite != null)
                {
                    Repository.Current = new Repository(_currentSite.AsActual().Repository);
                }
            }

            _filterContext = IPFilterContext.Current;
            _ipSetting = _filterContext.IPSettingService.GetSettings();
        }

        public void Do()
        {
            //if (AccessStaticFiles())
            //{
            //    return;
            //}

            if (_ipSetting != null)
            {
                bool filterFrontend = _ipSetting.FilterScope != (int) FilterScope.Backend;
                bool filterBackend = _ipSetting.FilterScope != (int) FilterScope.Frontend;

                if (filterFrontend || filterBackend)
                {
                    
                    var clientIp = GetClientIP();
                    var context = HttpContext.Current;
                    var path = context.Request.Path.ToLower();

                    if (IsFromBackEnd(path))
                    {
                        if (filterBackend)
                        {
                            DealWithIP(context, clientIp);
                        }
                    }
                    else
                    {
                        if (filterFrontend)
                        {
                            DealWithIP(context, clientIp);
                        }
                    }

                }
            }
        }

        private  bool IsFromBackEnd(string path)
        {
            return (path.StartsWith("/sites") || path.StartsWith("/account") || path.StartsWith("/contents"));
        }

        private  void DealWithIP(HttpContext context, string clientIp)
        {
            bool isLegalIP = true;

            if (_ipSetting.FilterType == (int) FilterType.Whitelist)
            {
                var wl = GetIPlist(FilterType.Whitelist);
                if (wl == null)
                {
                    isLegalIP = false;
                }
                else
                {
                    isLegalIP = wl.Contains(clientIp);
                }
            }
            else
            {
                var wl = GetIPlist(FilterType.Blacklist);
                if (wl == null)
                {
                    isLegalIP = true;
                }
                else
                {
                    isLegalIP = !wl.Contains(clientIp);
                }
            }

            if (!isLegalIP)
            {
                var forbiddenHtml = _ipSetting.ForbiddenHtml;;
                if (String.IsNullOrWhiteSpace(forbiddenHtml))
                {
                    forbiddenHtml = "<h1>Forbidden. </h1>";
                }

                context.Response.Clear();
                context.Response.Headers["IPValidation"] = "Forbidden";
                context.Response.ContentType = "text/html";
                context.Response.Write(forbiddenHtml);
                context.Response.End();
            }
        }

        private  List<string> GetIPlist(FilterType filterType)
        {
            var ipList = new List<string>();

            try
            {
                var service = _filterContext.IPListService;
                var list = service.GetIPList();
                if (list.Any())
                {
                    ipList.AddRange(list);
                }

                if (filterType == FilterType.Whitelist)
                {
                    ipList.Add("::1");
                    ipList.Add("127.0.0.1");
                }
            }
            catch (Exception ex)
            {
            }

            return ipList;
        }
 
        private  string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrWhiteSpace(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (String.IsNullOrWhiteSpace(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            return result;
        }

        private  bool AccessStaticFiles()
        {
            var requestUrl = HttpContext.Current.Request.Url.AbsolutePath.ToLower();

            var fileName = System.IO.Path.GetExtension(requestUrl);

            if (allowFiles.Contains(fileName))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private  string[] allowFiles = new string[] { ".js", ".jpg", ".png", ".svg" };
    }
}