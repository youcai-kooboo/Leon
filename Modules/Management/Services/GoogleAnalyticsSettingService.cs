using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services
{
    public class GoogleAnalyticsSettingService : SettingService
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.GoogleAnalyticsSetting;
            }
        }

        public string GetGoogleAnalytics()
        {
            return GetSettingValue("GoogleAnalytics", "<!-- Google analytics code -->");
        }
    }
}