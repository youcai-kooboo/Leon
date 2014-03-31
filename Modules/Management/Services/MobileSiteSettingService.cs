using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services
{
    public class MobileSiteSettingService : SettingService
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.MobileSiteSetting;
            }
        }

        public string Telephone
        {
            get
            {
                return GetSettingValue("Telephone");
            }
        }

        public string FacebookLike
        {
            get
            {
                return GetSettingValue("FacebookLike");
            }
        }

        public string DesktopWebsite
        {
            get
            {
                return GetSettingValue("DesktopWebsite", "http://www.budgetair.nl?from=m");
            }
        }

        //public bool HttpsEnabled() {
        //    var set = GetSettingValue("HttpsEnabled", "false");
        //    return string.Equals(set.Trim(), "true", StringComparison.OrdinalIgnoreCase);
        //}

        public bool PriceBeforeSymbol()
        {
            var set = GetSettingValue("PriceBeforeSymbol", "false");
            return string.Equals(set.Trim(), "true", StringComparison.OrdinalIgnoreCase);
        }

        public string BookExtraUrl
        {
            get
            {
                return GetSettingValue("feedback 1 URL");
            }
        }
        public string ShareTipUrl
        {
            get
            {
                return GetSettingValue("feedback 2 URL");
            }
        }
        public string NewsletterDefaultChecked
        {
            get
            {
                return GetSettingValue("NewsletterDefaultChecked", "false");
            }
        }

        #region tagman

        public string TagManEnabled
        {
            get
            {
                return GetSettingValue("TagManEnabled", "false");
            }
        }
        public string TagManClient
        {
            get
            {
                return GetSettingValue("TagManClient");
            }
        }
        public string TagManSiteId
        {
            get
            {
                return GetSettingValue("TagManSiteId");
            }
        }

        #endregion

        #region localytics

        public bool LocalyticsEnabled
        {
            get
            {
                var str = GetSettingValue("LocalyticsEnabled", "false");
                return string.Equals(str.Trim(), "true", StringComparison.OrdinalIgnoreCase);
            }
        }

        public string LocalyticsAppKey
        {
            get
            {
                return GetSettingValue("LocalyticsAppKey", "");
            }
        }

        #endregion
    }
}