using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leon.Modules.IPFilter.Services;
using Kooboo.CMS.Toolkit;


namespace Leon.Modules.IPFilter
{
    public class IPFilterContext : ContextBase
    {
        public static IPFilterContext Current
        {
            get
            {
                return ContextUtility.GetOrAdd<IPFilterContext>("IPFilterContext", () => new IPFilterContext());
            }
        }
        public IPSettingService IPSettingService
        {
            get
            {
                return this.GetService<IPSettingService>();
            }
        }
        public IPListService IPListService
        {
            get
            {
                return this.GetService<IPListService>();
            }
        }
    }
}