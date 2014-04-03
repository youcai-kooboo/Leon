using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class HourlyPv
    {
        public DateTime DateHour { get; set; }
        public int Total { get; set; }
        public int Error { get; set; }
    }
}
