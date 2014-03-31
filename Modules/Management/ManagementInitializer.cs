using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Toolkit;
using Kooboo.CMS.Toolkit.Modules;
using Kooboo.CMS.Sites.Models;
using Leon.Modules.Management;

namespace Leon.Modules.Management
{
    public class ManagementInitializer : IModuleInitializer
    {
        private const string ModuleName = "Management";
        private const string RepositoryName = "Management";

        #region IModuleInitializer Members

        public void Include(string moduleName, string siteName)
        {
            if(moduleName.Equals(ModuleName, StringComparison.OrdinalIgnoreCase))
            {
                /*
                    Copy settings from root site
                 */

                var currentSite = new Site(siteName);
                currentSite.AsActual();

                var rootSite = currentSite.RootSite();
                if(!currentSite.FullName.Equals(rootSite.FullName, StringComparison.OrdinalIgnoreCase))
                {
                    var rootSiteSettings = ManagementContext.Current.SettingService.GetSettings(rootSite.FullName);
                    var currentSiteSettings = ManagementContext.Current.SettingService.GetSettings(currentSite.FullName);

                    foreach(var rootSiteSetting in rootSiteSettings)
                    {
                        var currentSiteSetting = currentSiteSettings.FirstOrDefault(it =>
                            it.FolderName.Equals(rootSiteSetting.FolderName, StringComparison.OrdinalIgnoreCase) &&
                            it.Key.Equals(rootSiteSetting.Key, StringComparison.OrdinalIgnoreCase));

                        if(currentSiteSetting == null)
                        {
                            currentSiteSetting = rootSiteSetting;
                            currentSiteSetting.UUID = UUIDGenerator.Generate();
                            currentSiteSetting.Site = currentSite.FullName;

                            var folder = Kooboo.CMS.Content.Models.ContentExtensions.GetFolder(currentSiteSetting);
                            folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);
                            ManagementContext.Current.SettingService.Add(currentSiteSetting, folder);
                        }
                        else
                        {
                            currentSiteSetting.Value = rootSiteSetting.Value;
                            var folder = Kooboo.CMS.Content.Models.ContentExtensions.GetFolder(currentSiteSetting);
                            folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);
                            ManagementContext.Current.SettingService.Update(currentSiteSetting, folder);
                        }
                    }

                    //ManagementContext.Current.SettingService.ClearSettingsCache(currentSite.FullName);
                }
            }
        }

        public void Exclude(string moduleName, string siteName)
        { }

        public void UnInstall(string moduleName)
        { }

        #endregion
    }
}