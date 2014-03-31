using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services
{
    public class IPadRedirectSettingService : SettingService
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.IPadRedirectSetting;
            }
        }

        public string GetIPadRedirect()
        {
            return GetSettingValue("IPadRedirect");
        }
    }
}