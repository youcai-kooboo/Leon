using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class HealthStatistics
    {
        public int TotalPage { get; set; }
        public int VisitedPage { get; set; }
        public int ErrorPage { get; set; }

        public int TotalJs { get; set; }
        public int VisitedJs { get; set; }
        public int ErrorJs { get; set; }

        public int TotalImage { get; set; }
        public int VisitedImage { get; set; }
        public int ErrorImage { get; set; }

        public int TotalCss { get; set; }
        public int VisitedCss { get; set; }
        public int ErrorCss { get; set; }

        public Dictionary<int, int> StatusAndTotal { get; set; }
    }
}