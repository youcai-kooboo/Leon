using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services
{
    public class MobileRedirectSettingService : SettingService
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.MobileRedirectSetting;
            }
        }

        public string GetMobileRedirect()
        {
            return GetSettingValue("MobileRedirect");
        }
    }
}