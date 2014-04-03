using Kooboo.Web.Mvc.Grid2;
using Kooboo.Web.Mvc;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.ModuleArea.Monitor.Grid.Log
{
    public class VisitCountGridItemColumn:GridItemColumn
    {
        public VisitCountGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }

        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            if (DataItem is Models.Log)
            {
                var data = (Models.Log)DataItem;
                if (data.VisitCount > 0 && !string.IsNullOrEmpty(data.UniqueVisitorKey))
                {
                    var linkText = data.VisitCount.ToString();
                    var url = viewContext.UrlHelper().Action(null,
                        viewContext.RequestContext.AllRouteValues()
                        .Merge("uvKey", data.UniqueVisitorKey)
                        .Merge("page", 1));

                    return new HtmlString(string.Format("<a href='{0}'><img class='icon {2}' src='{3}'/> {1}</a>", url, linkText,
                        this.Class, Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Images/invis.gif")));
                }
            }

            return new HtmlString(this.PropertyValue.ToString());
        }
        protected virtual string Class
        {
            get { return ""; }
        }
    }
}
