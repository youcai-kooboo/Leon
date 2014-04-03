using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class DayPV
    {
        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int Error { get; set; }

        public int Normal
        {
            get { return this.Total - this.Error; }
        }
    }
}
