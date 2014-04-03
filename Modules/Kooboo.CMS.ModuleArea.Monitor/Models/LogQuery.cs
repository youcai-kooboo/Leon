using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class LogQuery
    {
        public string SiteName { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Method { get; set; }
        public string UserName { get; set; }
        public string ClientIP { get; set; }
        public int? Status { get; set; }
        public string SortField { get; set; }
        public string SortDir { get; set; }
        public string UVKey { get; set; }
    }
}
