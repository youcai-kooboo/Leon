using System;
using System.Collections.Generic;
using System.Linq;
using Kooboo.CMS.Toolkit;
using Kooboo.CMS.Toolkit.Services;
using Leon.Modules.IPFilter.Models;

namespace Leon.Modules.IPFilter.Services
{
    public class IPListService : ServiceBase<IPList>
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.IPList;
            }
        }

        public List<string> GetIPList()
        {
            return base.CreateQuery()
                .Where(m => m.Published == true)
                .MapTo<IPList>()
                .Select(m=>m.IP)
                .ToList();
        }
    }
}
