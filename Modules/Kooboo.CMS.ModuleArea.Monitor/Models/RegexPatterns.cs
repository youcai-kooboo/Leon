using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class RegexPatterns
    {
        public const string KoobooMonitor = @"(?<Browser>(Kooboo/Monitor))";
        public const string IE = @"(?<Browser>(MSIE))";
        public const string Firefox = @"(?<Browser>(Firefox))";
        public const string Chrome = @"(?<Browser>(Chrome))";
        public const string Safari = @"(?<Browser>(Safari))";
        public const string Opera = @"(?<Browser>(Opera))";


        public const string MetaChartset = @"Charset=(?<Encoding>.*)(;{0,1})";
    }
}
