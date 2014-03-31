//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.ServiceModel;
//using System.IO;
//using System.Text.RegularExpressions;
//using System.Web;
//using Kooboo.Web.Url;
//using Kooboo.CMS.Sites.View;
//using Kooboo.CMS.Sites.Web;
//using Kooboo.CMS.Sites.Models;
//using Kooboo.CMS.Toolkit;
//using Kooboo.CMS.Toolkit.HtmlResolver;
//using Leon.Modules.Management.ContentService;
//using Kooboo.CMS.Common.Persistence.Non_Relational;

//namespace Leon.Modules.Management.Services
//{
//    public class SynchronizeContentService : SettingService
//    {
//        private const string DefaultEndpointConfigurationName = "BasicHttpBinding_IContentService";

//        public override string FolderName
//        {
//            get
//            {
//                return FolderNames.SynchronizeContentSetting;
//            }
//        }

//        public SynchronizeContentService()
//        {
//            ServerAddress = GetSettingValue("ServerAddress", "http://services1.dev.travix.nl/ATSCurrent/ContentService.svc");
//            ApiUserName = GetSettingValue("ApiUserName", "BudgetairAPI");
//            ApiPassword = GetSettingValue("ApiPassword", "BudgetairAPIPassword");
//            LanguageId = GetSettingValue("LanguageId", "1043").AsInt();
//            Prefix = GetSettingValue("Prefix", "");
//        }

//        public string ServerAddress
//        {
//            get;
//            private set;
//        }

//        public string ApiUserName
//        {
//            get;
//            private set;
//        }

//        public string ApiPassword
//        {
//            get;
//            private set;
//        }

//        public int LanguageId
//        {
//            get;
//            private set;
//        }

//        public string Prefix
//        {
//            get;
//            private set;
//        }

//        private ContentServiceClient _client;
//        public ContentServiceClient Client
//        {
//            get
//            {
//                if (_client == null)
//                {
//                    _client = new ContentServiceClient(DefaultEndpointConfigurationName);
//                    _client.Endpoint.Address = new EndpointAddress(
//                        new Uri(ServerAddress),
//                        _client.Endpoint.Address.Identity,
//                        _client.Endpoint.Address.Headers);
//                }
//                return _client;
//            }
//        }

//        private ApiContext _apiContext;
//        public ApiContext ApiContext
//        {
//            get
//            {
//                if (_apiContext == null)
//                {
//                    _apiContext = new ApiContext();
//                    _apiContext.ApiUserName = ApiUserName;
//                    _apiContext.ApiPassword = ApiPassword;
//                    _apiContext.LanguageId = LanguageId;

//                }
//                return _apiContext;
//            }
//        }
//        /// <summary>
//        /// Add prefix for html class name
//        /// </summary>
//        /// <param name="source"></param>
//        /// <returns></returns>
//        public IHtmlString PrependPrefix(String source)
//        {
//            if (!string.IsNullOrEmpty(Prefix))
//            {
//                var pattern = "<\\S[^><]* class=[\"|'](.*?)[\"|'].*?>";
//                var classFormat = "class=\"{0}\"";
//                var regex = new Regex(pattern);
//                Match m = regex.Match(source);
//                while (m.Success)
//                {
//                    Group group = m.Groups[1];
//                    var oldClass = group.Value;
//                    var newClass = string.Join(" ", oldClass.SplitRemoveEmptyEntries(' ').Select(o => Prefix + o));
//                    source = source.Replace(classFormat.FormatWith(oldClass), classFormat.FormatWith(newClass));
//                    m = m.NextMatch();
//                }
//            }
//            return new HtmlString(source);
//        }

//        public void SynchronizeHeader()
//        {
//            PageContextUtility.InitPageContext();
//            var htmlHelper = MvcUtility.GetHtmlHelper();
//            htmlHelper.ViewData["ThirdParty"] = true;
//            var header = htmlHelper.FrontHtml().RenderView("Builtin.Header", htmlHelper.ViewData);

//            Site site = Site.Current;
//            string host = String.Empty;
//            string resourceDomain = site.ResourceDomain;
//            if (Page_Context.Current.PageRequestContext.RequestChannel == FrontRequestChannel.Host ||
//                Page_Context.Current.PageRequestContext.RequestChannel == FrontRequestChannel.HostNPath)
//            {
//                host = site.Domains.First();
//            }
//            else
//            {
//                host = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Headers["Host"];
//            }
//            header = PrependPrefix(AbsoluteUrlHtmlResolver.Resolve(header, host, resourceDomain).ToString());

//            // CDN issue
//            //string cssFileVirtualPath = Page_Context.Current.Url.FrontUrl().ThemeFileUrl("header_footer/header_footer.css").ToString();
//            bool cssFileExists = false;
//            string cssFileVirtualPath = String.Empty;
//            string cssFilePhysicalPath = String.Empty;

//            do
//            {
//                site = site.AsActual();

//                Theme theme = new Theme(site, site.Theme).LastVersion();
//                cssFileVirtualPath = UrlUtility.Combine(theme.VirtualPath, "header_footer/header_footer.css");
//                cssFilePhysicalPath = UrlUtility.MapPath(cssFileVirtualPath);

//                if (!String.IsNullOrEmpty(Prefix))
//                {
//                    var prefixDir = Path.GetDirectoryName(cssFilePhysicalPath);
//                    var prefixPhysicalPath = Path.Combine(prefixDir, Prefix + "header_footer.css");
//                    if (File.Exists(prefixPhysicalPath))
//                    {
//                        cssFilePhysicalPath = prefixPhysicalPath;
//                    }
//                }

//                cssFileExists = File.Exists(cssFilePhysicalPath);

//                site = theme.Site.Parent;
//            } while (site != null && !cssFileExists);

//            if (cssFileExists)
//            {
//                using (var reader = new StreamReader(cssFilePhysicalPath))
//                {
//                    MatchEvaluator urlReplacer = new MatchEvaluator(delegate(Match m)
//                    {
//                        string url = m.Value;
//                        if (url.EndsWith(".htc"))
//                        {
//                            return url;
//                        }
//                        Uri uri = new Uri(url, UriKind.RelativeOrAbsolute);
//                        if (uri.IsAbsoluteUri || VirtualPathUtility.IsAbsolute(url))
//                        { }
//                        else
//                        {
//                            url = VirtualPathUtility.Combine(cssFileVirtualPath, url);
//                        }

//                        if (String.IsNullOrEmpty(resourceDomain))
//                        {
//                            url = HttpContext.Current.Request.ToAbsoluteUrl(host, url);
//                        }
//                        else
//                        {
//                            url = UrlUtility.ToHttpAbsolute(resourceDomain, url);
//                        }
//                        return url;
//                    });

//                    string cssFileContent = reader.ReadToEnd();
//                    cssFileContent = Regex.Replace(
//                        cssFileContent,
//                        @"(?<=url\(\s*([""']?))(?<url>[^'""]+?)(?=\1\s*\))",
//                        urlReplacer,
//                        RegexOptions.IgnoreCase);

//                    header = new HtmlString(String.Format("<style type=\"text/css\">\r\n{0}\r\n</style>\r\n{1}", cssFileContent, header));
//                }
//            }

//            Client.SynchronizeContent(
//                ApiContext,
//                header.ToString(),
//                ContentType.Header);
//        }

//        public void SynchronizeFooter()
//        {
//            PageContextUtility.InitPageContext();
//            var htmlHelper = MvcUtility.GetHtmlHelper();
//            htmlHelper.ViewData["ThirdParty"] = true;
//            var footer = htmlHelper.FrontHtml().RenderView("Builtin.Footer", htmlHelper.ViewData);

//            Site site = Site.Current;
//            string host = String.Empty;
//            string resourceDomain = site.ResourceDomain;
//            if (Page_Context.Current.PageRequestContext.RequestChannel == FrontRequestChannel.Host ||
//                Page_Context.Current.PageRequestContext.RequestChannel == FrontRequestChannel.HostNPath)
//            {
//                host = Page_Context.Current.PageRequestContext.Site.Domains.First();
//            }
//            else
//            {
//                host = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Headers["Host"];
//            }

//            footer = PrependPrefix(AbsoluteUrlHtmlResolver.Resolve(footer, host, resourceDomain).ToString());

//            Client.SynchronizeContent(
//                ApiContext,
//                footer.ToString(),
//                ContentType.Footer);
//        }
//    }
//}