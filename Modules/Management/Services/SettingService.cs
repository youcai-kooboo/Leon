using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Toolkit;
using Leon.Modules.Management.Models;

namespace Leon.Modules.Management.Services
{
    public class SettingService : ServiceBase<Setting>
    {
        private static string SettingsCacheKeyFormat = "Management.Settings.{0}";
        private static object LockObj = new object();

        private static string SettingsCacheKey
        {
            get
            {
                return SettingsCacheKeyFormat.FormatWith(Site.Current.FullName);
            }
        }

        //public void ClearSettingsCache()
        //{
        //    ClearSettingsCache(Site.Current.FullName);
        //}

        //public void ClearSettingsCache(string siteFullName)
        //{
        //    CacheUtility.Remove(SettingsCacheKeyFormat.FormatWith(siteFullName));
        //}

        public override IContentQuery<TextContent> CreateQuery()
        {
            return base.CreateQuery()
                .WhereEquals(Setting.FieldNames.Site, Site.Current.FullName);
        }

        public override Setting Get(TextContent textContent)
        {
            if(textContent != null)
            {
                return new Setting(textContent);
            }
            return null;
        }

        /// <summary>
        /// Get setting from current site and current folder
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Setting GetSetting(string key)
        {
            //return this.Get(Setting.FieldNames.Key, key);
            return GetSettings().FirstOrDefault(it =>
                it.FolderName.Equals(this.FolderName, StringComparison.OrdinalIgnoreCase) &&
                it.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get setting from current site and current folder
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSettingValue(string key)
        {
            return GetSettingValue(key, String.Empty);
        }

        /// <summary>
        /// Get setting from current site and current folder
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetSettingValue(string key, string defaultValue)
        {
            Setting setting = GetSetting(key);
            if(setting == null)
            {
                lock(LockObj)
                {
                    if(setting == null)
                    {
                        setting = new Setting()
                        {
                            Site = Site.Current.FullName,
                            Key = key,
                            Value = defaultValue,
                            Published = true
                        };
                        base.Add(setting);
                    }
                }
            }

            if(setting.Published.HasValue && setting.Published.Value)
            {
                return setting.Value;
            }

            return String.Empty;
        }

        /// <summary>
        /// Get settings from current site include all folders
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Setting> GetSettings()
        {
            return GetSettings(Site.Current.FullName);
        }

        /// <summary>
        /// Get settings from site include all folders
        /// </summary>
        /// <param name="siteFullName"></param>
        /// <returns></returns>
        public IEnumerable<Setting> GetSettings(string siteFullName)
        {
            //string settingsCacheKey = SettingsCacheKeyFormat.FormatWith(siteFullName);

            //var settings = CacheUtility.Get(settingsCacheKey) as IEnumerable<Setting>;
            //if(settings == null)
            //{
              var  settings = Schema.CreateQuery()
                    .WhereEquals(Setting.FieldNames.Site, siteFullName)
                    .Select(textContent => Get(textContent))
                    .ToList();

                //CacheUtility.Add(settingsCacheKey, settings);
            //}

            return settings;
        }
    }
}