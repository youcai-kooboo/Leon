using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Toolkit;
using Leon.Modules.Management.Models;

namespace Leon.Modules.Management.Services
{
    public class RedirectBasedOnBrowserLanguageService : ServiceBase<RedirectBasedOnBrowserLanguage>
    {
        private static string RedirectSettingsCacheKeyFormat = "Management.RedirectSettings.{0}";

        private static string RedirectSettingsCacheKey
        {
            get
            {
                return RedirectSettingsCacheKeyFormat.FormatWith(Site.Current.FullName);
            }
        }

        public void ClearRedirectSettingsCache()
        {
            ClearRedirectSettingsCache(Site.Current.FullName);
        }

        public void ClearRedirectSettingsCache(string siteFullName)
        {
            CacheUtility.Remove(RedirectSettingsCacheKeyFormat.FormatWith(siteFullName));
        }

        public override string FolderName
        {
            get
            {
                return FolderNames.RedirectBasedOnBrowserLanguage;
            }
        }

        public override IContentQuery<TextContent> CreateQuery()
        {
            return base.CreateQuery()
                .WhereEquals(RedirectBasedOnBrowserLanguage.FieldNames.Site, Site.Current.FullName);
        }

        public override RedirectBasedOnBrowserLanguage Get(TextContent textContent)
        {
            if(textContent != null)
            {
                return new RedirectBasedOnBrowserLanguage(textContent);
            }
            return null;
        }

        public IEnumerable<RedirectBasedOnBrowserLanguage> GetRedirectSettings()
        {
            return GetRedirectSettings(Site.Current.FullName);
        }

        public IEnumerable<RedirectBasedOnBrowserLanguage> GetRedirectSettings(string siteFullName)
        {
            string redirectSettingsCacheKey = RedirectSettingsCacheKeyFormat.FormatWith(siteFullName);

            var redirectSettings = CacheUtility.Get(redirectSettingsCacheKey) as IEnumerable<RedirectBasedOnBrowserLanguage>;
            if(redirectSettings == null)
            {
                redirectSettings = GetAll().ToList();
                CacheUtility.Add(redirectSettingsCacheKey, redirectSettings);
            }

            return redirectSettings;
        }
    }
}