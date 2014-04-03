using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Gird
{
    public class GridItemBase:GridItem
    {
        public GridItemBase(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {

        }

        private ModuleInfo_Metadata _moduleInfo;
        protected ModuleInfo_Metadata ModuleInfo
        {
            get
            {
                if (this._moduleInfo == null)
                {
                    this._moduleInfo = new ModuleInfo_Metadata(MonitorAreaRegistration.ModuleName, Sites.Models.Site.Current.Name);
                }
                return this._moduleInfo;
            }
        }
    }
}
