using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services
{

    public class CheckoutSiteSettingService : SettingService
    {

        public override string FolderName
        {
            get
            {
                return FolderNames.CheckoutSiteSetting;
            }
        }

        public bool PriceBeforeSymbol()
        {
            var set = GetSettingValue("PriceBeforeSymbol", "false");
            return string.Equals(set.Trim(), "true", StringComparison.OrdinalIgnoreCase);
        }

        public string CurrentLanguageCountryCode()
        {
            var code = GetSettingValue("CurrentLanguageCountryCode", "NL");
            return (code ?? string.Empty).Trim();
        }

        public bool EnableCalendarMatrix()
        {
            return string.Equals("true", GetSettingValue("EnableCalendarMatrix", "False"), StringComparison.OrdinalIgnoreCase);
        }
    }
}
