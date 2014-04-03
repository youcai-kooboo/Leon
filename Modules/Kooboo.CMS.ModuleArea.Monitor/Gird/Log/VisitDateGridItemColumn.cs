using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Grid.Log
{
    public class VisitDateGridItemColumn:GridItemColumn
    {
        public VisitDateGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            string str = string.Empty;
            if (PropertyValue != null)
            {
                DateTime visitDate = (DateTime)PropertyValue;
                DateTime utcNow = DateTime.UtcNow;
                TimeSpan ts = utcNow.Subtract(visitDate);
                if (ts.Days > 0)
                {
                    str = visitDate.ToLocalTime().ToString("yyyy-MM-dd hh:mm:ss");
                }
                else if (ts.Hours > 0)
                {
                    str = string.Format("{0} hours ago", ts.Hours);
                }
                else if (ts.Minutes > 0)
                {
                    str = string.Format("{0} minutes ago", ts.Minutes);
                }
                else if (ts.Seconds > 0)
                {
                    str = string.Format("{0} seconds ago", ts.Seconds);
                }
                else if (ts.Milliseconds > 0)
                {
                    str = "1 seconds ago";
                }
            }
            return new HtmlString(str);
        }
    }
}
