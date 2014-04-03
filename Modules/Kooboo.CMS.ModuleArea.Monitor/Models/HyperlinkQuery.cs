using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class HyperlinkQuery
    {
        public string SiteName { get; set; }
        public string Url { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string ResourceType { get; set; }
        public string Type { get; set; }
        public string SortField { get; set; }
        public string SortDir { get; set; }
        public int? Status { get; set; }
    }
}
