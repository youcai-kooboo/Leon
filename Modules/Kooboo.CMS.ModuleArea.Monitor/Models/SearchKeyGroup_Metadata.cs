using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    [MetadataFor(typeof(SearchKeyGroup))]
    [Grid]
    public class SearchKeyGroup_Metadata
    {
        [GridColumn]
        public int Count { get; set; }

        [GridColumn]
        public string Keyword { get; set; }
    }
}
