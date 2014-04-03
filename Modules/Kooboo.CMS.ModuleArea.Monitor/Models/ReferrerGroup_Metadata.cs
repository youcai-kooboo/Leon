using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.ModuleArea.Monitor.Grid;
using Kooboo.CMS.ModuleArea.Monitor.Grid.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    [MetadataFor(typeof(ReferrerGroup))]
    [Grid]
    public class ReferrerGroup_Metadata
    {
        [GridColumn]
        public int Count { get; set; }

        [GridColumn(GridItemColumnType=typeof(ReferrerJumpGridActionItemColumn))]
        public string Url { get; set; }
    }
}
