using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Grid
{
    public class DoubleGridItemColumn : GridItemColumn
    {
        public DoubleGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            string str = string.Empty;
            if (PropertyValue != null)
            {
                str = ((double)PropertyValue).ToString("N4");
            }
            return new HtmlString(str);
        }
    }
}
