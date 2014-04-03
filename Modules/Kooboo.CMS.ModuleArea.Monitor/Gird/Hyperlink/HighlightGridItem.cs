using Kooboo.CMS.ModuleArea.Monitor.Gird;
using Kooboo.Globalization;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.ModuleArea.Monitor.Grid.Hyperlink
{
    public class HighlightGridItem:GridItemBase
    {
        public HighlightGridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {

        }

        public override IHtmlString RenderItemContainerAtts()
        {
            StringBuilder sb = new StringBuilder();
            if (DataItem is Models.Hyperlink)
            {
                
                var data = DataItem as Models.Hyperlink;
                if (data.Status >= 400)
                {
                    sb.Append("class=\"error-status\"");
                }
                else if (this.ModuleInfo.MaxTimeTaken > 0 && data.VisitTime >= this.ModuleInfo.MaxTimeTaken)
                {
                    sb.Append("class=\"error-overtime\"");
                }
            }
            return new HtmlString(sb.ToString());
        }
    }
}
