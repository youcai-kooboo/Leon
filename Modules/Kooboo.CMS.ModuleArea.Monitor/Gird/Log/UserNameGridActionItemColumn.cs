using Kooboo.Web.Mvc.Grid2;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.ModuleArea.Monitor.Grid.Log
{
    public class UserNameGridActionItemColumn:GridItemColumn
    {
        public UserNameGridActionItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            if (DataItem is Models.Log)
            {
                if (this.PropertyValue != null && !string.IsNullOrWhiteSpace(this.PropertyValue.ToString()))
                {
                    var data = (Models.Log)DataItem;
                    var linkText = data.UserName;
                    var @class = Class;
                    var url = viewContext.UrlHelper().Action(null,
                        viewContext.RequestContext.AllRouteValues()
                        .Merge("UserName", data.UserName)
                        .Merge("page", 1));

                    return new HtmlString(string.Format("<a href='{0}'><img class='icon {2}' src='{3}'/> {1}</a>", url, linkText,
                        @class, Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Images/invis.gif")));
                }
            }

            return new HtmlString("-");
        }
        protected virtual string Class
        {
            get { return ""; }
        }
    }
}