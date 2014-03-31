using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.View;

namespace Leon.Plugins
{
    public abstract class PluginBase : IPagePlugin
    {
        private string _description;
        public virtual string Description
        {
            get
            {
                if (String.IsNullOrEmpty(_description))
                {
                    _description = this.GetType().Name;
                }

                return _description;
            }
        }

        public Page_Context PageContext
        {
            get;
            private set;
        }

        public PagePositionContext PagePositionContext
        {
            get;
            private set;
        }

        public HttpContextBase Context
        {
            get;
            private set;
        }

        public HttpRequestBase Request
        {
            get;
            private set;
        }

        public HttpResponseBase Response
        {
            get;
            private set;
        }

        private ViewDataDictionary _viewData;
        public ViewDataDictionary ViewData
        {
            get
            {
                if (_viewData == null)
                {
                    _viewData = PageContext.ControllerContext.Controller.ViewData;
                }

                return _viewData;
            }
        }

        private dynamic _viewBag;
        public dynamic ViewBag
        {
            get
            {
                if (_viewBag == null)
                {
                    _viewBag = PageContext.ControllerContext.Controller.ViewBag;
                }

                return _viewBag;
            }
        }

        private TempDataDictionary _tempData;
        public TempDataDictionary TempData
        {
            get
            {
                if (_tempData == null)
                {
                    _tempData = PageContext.ControllerContext.Controller.TempData;
                }

                return _tempData;
            }
        }

        public bool IsPost
        {
            get
            {
                return Request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase);
            }
        }

        private NameValueCollection _form;
        public NameValueCollection Form
        {
            get
            {
                if (_form == null)
                {
                    _form = Request.Unvalidated().Form;
                }

                return _form;
            }
        }

        public virtual ActionResult Execute(Page_Context pageContext, PagePositionContext positionContext)
        {
            PageContext = pageContext;
            PagePositionContext = positionContext;
            Context = pageContext.ControllerContext.HttpContext;
            Request = Context.Request;
            Response = Context.Response;

            return Execute();
        }

        public abstract ActionResult Execute();


        /// <summary>
        /// Redirects the specified page full name.
        /// </summary>
        /// <param name="pageFullName">Full name of the page.</param>
        /// <returns></returns>
        public virtual ActionResult Redirect(string pageFullName, string queryString = "")
        {
            var page = this.PageContext.FrontUrl.PageUrl(pageFullName);

            if (page != null)
            {
                return new RedirectResult(page + queryString);
            }

            return null;

        }
    }
}
