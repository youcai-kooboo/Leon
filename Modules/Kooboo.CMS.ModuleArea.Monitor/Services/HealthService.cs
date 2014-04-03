using HtmlAgilityPack;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Repositories;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Services
{
    public class HealthService
    {
        #region .ctor
        private string _siteName;
        private HealthService(string siteName)
        {
            this._siteName = siteName;
        }
        #endregion

        #region Properties
        public DateTime? Begin { get; private set; }
        public DateTime? End { get; private set; }
        public string EntryUrl { get; private set; }
        public int MaxDepth { get; private set; }
        public bool CheckImage { get; private set; }
        public bool CheckJs { get; private set; }
        public bool CheckCss { get; private set; }
        public int TotalPage { get; private set; }
        public int VisitedPage { get; private set; }
        public int ErrorPage { get; private set; }
        public int TotalImage { get; private set; }
        public int VisitedImage { get; private set; }
        public int ErrorImage { get; private set; }
        public int TotalCss { get; private set; }
        public int VisitedCss { get; private set; }
        public int ErrorCss { get; private set; }
        public int TotalJs { get; private set; }
        public int VisitedJs { get; private set; }
        public int ErrorJs { get; private set; }
        public RunType RunType { get; private set; }
        public HealthServiceState State { get; private set; }
        public bool IsRunning
        {
            get
            {
                return this.IsVisiting || this.IsChecking;
            }
        }
        public bool IsVisiting
        {
            get
            {
                return this.State == HealthServiceState.Visiting || this.State == HealthServiceState.VisitCanceling;
            }
        }
        public bool IsChecking
        {
            get
            {
                return this.State == HealthServiceState.Checking || this.State == HealthServiceState.CheckCanceling;
            }
        }
        private CancellationTokenSource CancelToKen { get; set; }
        private Exception Exception { get; set; }
        private HashSet<string> _links = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly object _locker = new object();
        #endregion

        #region ReCheck
        public void ReCheck()
        {
            lock (_locker)
            {
                if (this.IsRunning)
                {
                    return;
                }
                this.RunType = RunType.Check;
                this.State = HealthServiceState.Checking;
                this.Reset();
                this.Begin = DateTime.UtcNow;
                CancelToKen = new CancellationTokenSource();
                var service = EngineContext.Current.Resolve<HyperlinkService>();
                var task = new Task(() =>
                {
                    service.Clear();
                    var link = new Hyperlink();
                    link.ResourceType = ResourceType.PAGE;
                    link.Url = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(this.EntryUrl, " ");
                    TotalPage++;
                    var html = Visit(link);
                    this._links.Add(link.Url);
                    var lst = Extract(link, html, true, this.CheckImage, this.CheckJs, this.CheckCss);
                    link.ExtractQty = lst.Count;
                    service.Save(link);
                    service.Save(lst);
                    for (var i = 1; i <= this.MaxDepth; i++)
                    {
                        CancelToKen.Token.ThrowIfCancellationRequested();
                        var items = service.GetHyperlinksByDepth(i);
                        if (!items.Any())
                        {
                            break;
                        }
                        foreach (var item in items)
                        {
                            CancelToKen.Token.ThrowIfCancellationRequested();
                            html = Visit(item);
                            if (item.IsPage)
                            {
                                lst = Extract(item, html, i < this.MaxDepth, this.CheckImage, this.CheckJs, this.CheckCss);
                                item.ExtractQty = lst.Count;
                            }
                            service.Save(item);
                            if (item.IsPage && i < this.MaxDepth && lst.Any())
                            {
                                service.Save(lst);
                            }
                        }
                    }

                }, CancelToKen.Token, TaskCreationOptions.LongRunning);

                task.ContinueWith((it) =>
                {
                    if (it.IsCanceled)
                    {
                        this.State = HealthServiceState.CheckCanceled;
                    }
                    else
                    {
                        this.State = HealthServiceState.CheckCompleted;
                        if (it.Exception != null)
                        {
                            this.Exception = it.Exception;
                            Kooboo.HealthMonitoring.Log.LogException(this.Exception);
                        }
                    }
                    this.End = DateTime.UtcNow;
                    this._links.Clear();
                    service.Dispose();
                    CancelToKen.Dispose();
                    CancelToKen = null;
                });

                task.Start();
            }
        }
        #endregion

        #region ReVisit
        public void ReVisit(Hyperlink[] model)
        {
            lock (_locker)
            {
                if (this.IsRunning)
                {
                    return;
                }
                this.RunType = RunType.ReVisit;
                this.State = HealthServiceState.Visiting;
                this.Reset();
                this.Begin = DateTime.UtcNow;
                CancelToKen = new CancellationTokenSource();
                var service = EngineContext.Current.Resolve<HyperlinkService>();
                var task = new Task(() =>
                {
                    var ids = model.Select(it => it.Id).ToArray();
                    var lst = service.Get(ids);
                    this.TotalCss = lst.Count(it => it.ResourceType.Equals(ResourceType.CSS, StringComparison.OrdinalIgnoreCase));
                    this.TotalImage = lst.Count(it => it.ResourceType.Equals(ResourceType.IMAGE, StringComparison.OrdinalIgnoreCase));
                    this.TotalPage = lst.Count(it => it.ResourceType.Equals(ResourceType.PAGE, StringComparison.OrdinalIgnoreCase));
                    this.TotalJs = lst.Count(it => it.ResourceType.Equals(ResourceType.JAVASCRIPT, StringComparison.OrdinalIgnoreCase));
                    foreach (var item in lst)
                    {
                        CancelToKen.Token.ThrowIfCancellationRequested();
                        Visit(item);
                        service.Save(item);
                    }
                }, CancelToKen.Token, TaskCreationOptions.LongRunning);

                task.ContinueWith((it) =>
                {
                    if (it.IsCanceled)
                    {
                        this.State = HealthServiceState.VisitCanceled;
                    }
                    else
                    {
                        this.State = HealthServiceState.VisitCompleted;
                        if (it.Exception != null)
                        {
                            this.Exception = it.Exception;
                            Kooboo.HealthMonitoring.Log.LogException(this.Exception);
                        }
                    }
                    this.End = DateTime.UtcNow;
                    this._links.Clear();
                    service.Dispose();
                    CancelToKen.Dispose();
                    CancelToKen = null;
                });

                task.Start();
            }
        }
        #endregion

        #region Stop
        public void Stop()
        {
            if (CancelToKen != null && !CancelToKen.IsCancellationRequested)
            {
                this.State = HealthServiceState.CheckCanceling;
                CancelToKen.Cancel();
            }
        }
        #endregion

        private List<Hyperlink> Extract(Hyperlink parent, string html, bool overlying = false, bool checkImage = false, bool checkJs = false, bool checkCss = false)
        {
            var begin = DateTime.UtcNow;
            List<Hyperlink> lst = new List<Hyperlink>();
            Uri tmp = null;

            try
            {
                Uri parentUri = new Uri(parent.Url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNodeCollection anhorNodes = doc.DocumentNode.SelectNodes("//a[@href]|//iframe[@src]|//frame[@src]");

                string url = string.Empty;
                string tagName = string.Empty;
                if (anhorNodes != null)
                {
                    foreach (var node in anhorNodes)
                    {
                        tagName = node.Name;
                        if (tagName.Equals("a", StringComparison.OrdinalIgnoreCase))
                        {
                            url = node.GetAttributeValue("href", string.Empty).ToLower();
                        }
                        else if (tagName.Equals("iframe", StringComparison.OrdinalIgnoreCase) || tagName.Equals("frame", StringComparison.OrdinalIgnoreCase))
                        {
                            url = node.GetAttributeValue("src", string.Empty).ToLower();
                        }
                        if (ValidateUrl(url))
                        {
                            url = HttpUtility.HtmlDecode(url);
                            if (url.StartsWith("//"))
                            {
                                url = string.Format("{0}:{1}", parentUri.Scheme, url);
                            }
                            if (url.StartsWith("http://") || url.StartsWith("https://"))
                            {
                                tmp = new Uri(url);
                                if (!tmp.Authority.Equals(parentUri.Authority, StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                url = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(parentUri.AbsoluteUri, url);
                            }
                            if (this._links.Add(url))
                            {
                                var link = new Hyperlink();
                                link.ResourceType = ResourceType.PAGE;
                                link.InnerText = HttpUtility.HtmlDecode(node.InnerText);
                                link.Depth = parent.Depth + 1;
                                link.ParentUrl = parent.Url;
                                link.Url = url;
                                lst.Add(link);
                                if (overlying)
                                {
                                    TotalPage++;
                                }
                            }
                        }
                    }
                }
                if (checkImage)
                {
                    HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@src]");
                    if (imgNodes != null)
                    {
                        foreach (var node in imgNodes)
                        {
                            url = node.GetAttributeValue("src", string.Empty).ToLower();
                            if (ValidateUrl(url))
                            {
                                url = HttpUtility.HtmlDecode(url);
                                if (url.StartsWith("//"))
                                {
                                    url = string.Format("{0}:{1}", parentUri.Scheme, url);
                                }
                                if (url.StartsWith("http://") || url.StartsWith("https://"))
                                {
                                    tmp = new Uri(url);
                                    if (!tmp.Authority.Equals(parentUri.Authority, StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    url = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(parentUri.AbsoluteUri, url);
                                }
                                if (this._links.Add(url))
                                {
                                    var link = new Hyperlink();
                                    link.ResourceType = ResourceType.IMAGE;
                                    link.Depth = parent.Depth + 1;
                                    link.ParentUrl = parent.Url;
                                    link.Url = url;
                                    lst.Add(link);
                                    if (overlying)
                                    {
                                        TotalImage++;
                                    }
                                }
                            }
                        }
                    }
                }
                if (checkJs)
                {
                    HtmlNodeCollection jsNodes = doc.DocumentNode.SelectNodes("//script[@src]");
                    if (jsNodes != null)
                    {
                        foreach (var node in jsNodes)
                        {
                            url = node.GetAttributeValue("src", string.Empty).ToLower();
                            if (ValidateUrl(url))
                            {
                                url = HttpUtility.HtmlDecode(url);
                                if (url.StartsWith("//"))
                                {
                                    url = string.Format("{0}:{1}", parentUri.Scheme, url);
                                }
                                if (url.StartsWith("http://") || url.StartsWith("https://"))
                                {
                                    tmp = new Uri(url);
                                    if (!tmp.Authority.Equals(parentUri.Authority, StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    url = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(parentUri.AbsoluteUri, url);
                                }
                                if (this._links.Add(url))
                                {
                                    var link = new Hyperlink();
                                    link.ResourceType = ResourceType.JAVASCRIPT;
                                    link.Depth = parent.Depth + 1;
                                    link.ParentUrl = parent.Url;
                                    link.Url = url;
                                    lst.Add(link);
                                    if (overlying)
                                    {
                                        TotalJs++;
                                    }
                                }
                            }
                        }
                    }
                }
                if (checkCss)
                {
                    HtmlNodeCollection cssNodes = doc.DocumentNode.SelectNodes("//link[@href][@rel]|//link[@href][@type]");
                    if (cssNodes != null)
                    {
                        foreach (var node in cssNodes)
                        {
                            if (!string.Equals(node.GetAttributeValue("rel", string.Empty), "stylesheet", StringComparison.OrdinalIgnoreCase)
                                && !string.Equals(node.GetAttributeValue("type", string.Empty), "text/css", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }
                            url = node.GetAttributeValue("href", string.Empty).ToLower();
                            if (ValidateUrl(url))
                            {
                                url = HttpUtility.HtmlDecode(url);
                                if (url.StartsWith("//"))
                                {
                                    url = string.Format("{0}:{1}", parentUri.Scheme, url);
                                }
                                if (url.StartsWith("http://") || url.StartsWith("https://"))
                                {
                                    tmp = new Uri(url);
                                    if (!tmp.Authority.Equals(parentUri.Authority, StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    url = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(parentUri.AbsoluteUri, url);
                                }
                                if (this._links.Add(url))
                                {
                                    var link = new Hyperlink();
                                    link.ResourceType = ResourceType.CSS;
                                    link.Depth = parent.Depth + 1;
                                    link.ParentUrl = parent.Url;
                                    link.Url = url;
                                    lst.Add(link);
                                    if (overlying)
                                    {
                                        TotalCss++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            parent.ExtractTime = DateTime.UtcNow.Subtract(begin).TotalSeconds;
            return lst;
        }

        private string Visit(Hyperlink link)
        {
            var begin = DateTime.UtcNow;
            var html = string.Empty;
            var res = this.GetResponse(link.Url);
            if (res != null)
            {
                link.Status = (int)res.StatusCode;
                link.ContentLength = res.ContentLength;
                link.ContentType = res.ContentType;
                if (link.IsPage)
                {
                    try
                    {
                        Stream stream = res.GetResponseStream();
                        if (stream != null)
                        {
                            byte[] buffer = new byte[5000];
                            MemoryStream ms = new MemoryStream();
                            for (int i = stream.Read(buffer, 0, buffer.Length); i > 0; i = stream.Read(buffer, 0, buffer.Length))
                            {
                                ms.Write(buffer, 0, i);
                            }
                            stream.Close();
                            var data = ms.ToArray();
                            ms.Close();
                            var charset = res.CharacterSet;
                            var encoding = this.GetEncoding(charset) ?? Encoding.Default;
                            html = encoding.GetString(data, 0, data.Length);

                            charset = GetCharset(html);
                            if (!string.IsNullOrWhiteSpace(charset))
                            {
                                encoding = GetEncoding(charset);
                                if (encoding != null)
                                {
                                    html = encoding.GetString(data, 0, data.Length);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Kooboo.HealthMonitoring.Log.LogException(ex);
                    }
                }
                res.Close();
            }
            //visited
            switch (link.ResourceType)
            {
                case ResourceType.PAGE:
                    VisitedPage++;
                    break;
                case ResourceType.CSS:
                    VisitedCss++;
                    break;
                case ResourceType.IMAGE:
                    VisitedImage++;
                    break;
                case ResourceType.JAVASCRIPT:
                    VisitedJs++;
                    break;
            }
            //error
            if (link.Status == 0 || link.Status >= 400)
            {
                switch (link.ResourceType)
                {
                    case ResourceType.PAGE:
                        ErrorPage++;
                        break;
                    case ResourceType.CSS:
                        ErrorCss++;
                        break;
                    case ResourceType.IMAGE:
                        ErrorImage++;
                        break;
                    case ResourceType.JAVASCRIPT:
                        ErrorJs++;
                        break;
                }
            }
            link.UtcVisitDate = DateTime.UtcNow;
            link.VisitTime = DateTime.UtcNow.Subtract(begin).TotalSeconds;
            return html;
        }

        Regex charsetRegex = new Regex(Models.RegexPatterns.MetaChartset, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private string GetCharset(string html)
        {
            var charset = string.Empty;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var metaNodes = doc.DocumentNode.SelectNodes("//meta[@charset]|//meta[@content]");
            if (metaNodes != null)
            {
                foreach (var node in metaNodes)
                {
                    charset = node.GetAttributeValue("charset", string.Empty);
                    if (!string.IsNullOrWhiteSpace(charset))
                    {
                        break;
                    }
                    var content = node.GetAttributeValue("content", string.Empty);
                    var match = charsetRegex.Match(content);
                    if (match.Success)
                    {
                        charset = match.Groups["Encoding"].Value;
                        break;
                    }
                }
            }
            metaNodes = null;
            doc = null;//fix bug for HtmlAgilityPack "System.OutOfMemoryException":dispose HtmlDocument instance
            return charset;
        }

        private Encoding GetEncoding(string charset)
        {
            try
            {
                return Encoding.GetEncoding(charset);
            }
            catch { }
            return null;
        }

        private HttpWebResponse GetResponse(string url)
        {
            HttpWebRequest webReq = null;
            HttpWebResponse webRes = null;
            try
            {
                webReq = HttpWebRequest.Create(url) as HttpWebRequest;
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                webReq.UserAgent = Keys.KBM_USER_AGENT;
                webReq.Timeout = 10 * 1000;
                webRes = webReq.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                webRes = ex.Response as HttpWebResponse;
            }
            catch (Exception ex)
            {
                Kooboo.HealthMonitoring.Log.LogException(ex);
            }
            return webRes;
        }

        private bool ValidateUrl(string url)
        {
            return !(string.IsNullOrEmpty(url) || url.StartsWith("javascript:")
                || url.StartsWith("mailto:") || url.Contains("#")
                || url.StartsWith("file:"));
        }

        private void Reset()
        {
            this.Begin =
                this.End = null;
            this.Exception = null;
            this.TotalJs =
                this.TotalCss =
                this.TotalImage =
                this.TotalPage =
                this.VisitedCss =
                this.VisitedImage =
                this.VisitedJs =
                this.VisitedPage =
                this.ErrorCss =
                this.ErrorImage =
                this.ErrorJs =
                this.ErrorPage = 0;
            var moduleInfo = new ModuleInfo_Metadata(MonitorAreaRegistration.ModuleName, this._siteName);
            this.CheckImage = moduleInfo.CheckImage;
            this.CheckCss = moduleInfo.CheckCss;
            this.CheckJs = moduleInfo.CheckJavaScript;
            this.MaxDepth = moduleInfo.MaxDepth == 0 ? 3 : moduleInfo.MaxDepth;
            this.EntryUrl = moduleInfo.EntryUrl;
            this._links.Clear();
        }

        #region Static member
        private static Dictionary<string, HealthService> _services;
        static HealthService()
        {
            _services = new Dictionary<string, HealthService>(StringComparer.OrdinalIgnoreCase);
        }

        public static void Remove(string siteName)
        {
            _services.Remove(siteName.ToLower());
        }

        public static HealthService Get(string siteName)
        {
            HealthService service = null;
            if (_services.Keys.Contains(siteName))
            {
                service = _services[siteName];
            }
            else
            {
                service = new HealthService(siteName);
                _services.Add(siteName, service);
            }
            return service;
        }
        #endregion
    }

    public enum HealthServiceState
    {
        Stopped = 0,
        Checking = 1,
        CheckCompleted = 2,
        CheckCanceling = 3,
        CheckCanceled = 4,

        Visiting = 5,
        VisitCompleted = 6,
        VisitCanceling = 7,
        VisitCanceled = 8
    }

    public enum RunType
    {
        Check = 0,
        ReVisit = 1
    }
}
