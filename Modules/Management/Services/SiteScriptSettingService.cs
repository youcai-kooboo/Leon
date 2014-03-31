using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services
{
    public class SiteScriptSettingService : SettingService
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.SiteScript;
            }
        }

        public string GetBannerHeadScript()
        {
            return GetSettingValue("SiteScript_Head");
        }

        public string GetBannerBodyScript()
        {
            return GetSettingValue("SiteScript_BodyStart");
        }

        public string GetBannerFooterScript() {
            return GetSettingValue("SiteScript_BodyEnd");
        }
    }
}