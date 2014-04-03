#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.CMS.ModuleArea.Monitor.Models;

namespace Kooboo.CMS.ModuleArea.Monitor.Grid
{
    public class ReferrerJumpGridActionItemColumn : GridItemColumn
    {
        public ReferrerJumpGridActionItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            if (DataItem is ReferrerGroup)
            {
                var data = (ReferrerGroup)DataItem;
                var @class = Class;

                return new HtmlString(string.Format("<a target='_blank' title='{0}' href='{2}'><img class='icon {3}' src='{4}'/> {1}</a>",
                    HttpUtility.UrlDecode(data.Url),
                    curtailUrl(HttpUtility.UrlDecode(data.Url), 138), 
                    data.Url,
                    @class,
                    Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Images/invis.gif")));
            }

            return new HtmlString("-");
        }

        protected virtual string Class
        {
            get { return ""; }
        }

        private string curtailUrl(string url,int length)
        {
            if (string.IsNullOrWhiteSpace(url) || url.Length < length)
            {
                return url;
            }
            return url.Substring(0, length) + "...";
        }
    }
}