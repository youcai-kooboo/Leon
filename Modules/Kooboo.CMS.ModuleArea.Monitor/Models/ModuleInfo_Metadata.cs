#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class ModuleInfo_Metadata
    {
        public ModuleInfo_Metadata()
        {
            this.Settings = new ModuleSettings();
        }

        public ModuleInfo_Metadata(string moduleName, string siteName)
        {
            var cxt = new ModuleContext(moduleName, Kooboo.CMS.Sites.Models.SiteHelper.Parse(siteName));

            this.ModuleName = cxt.ModuleInfo.ModuleName;
            this.Version = cxt.ModuleInfo.Version;
            this.KoobooCMSVersion = cxt.ModuleInfo.KoobooCMSVersion;
            this.Settings = cxt.GetModuleSettings();
            if (this.Settings.CustomSettings.Keys.Contains(Keys.MODULE_CRAWL_DEPTH))
            {
                var str = this.Settings.CustomSettings[Keys.MODULE_CRAWL_DEPTH];
                this.MaxDepth = string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
            }
            if (this.Settings.CustomSettings.Keys.Contains(Keys.MODULE_CRAWL_ENTRY))
            {
                this.EntryUrl = this.Settings.CustomSettings[Keys.MODULE_CRAWL_ENTRY];
            }
            if (this.Settings.CustomSettings.Keys.Contains(Keys.MODULE_CHECK_IMAGE))
            {
                var str = this.Settings.CustomSettings[Keys.MODULE_CHECK_IMAGE];
                this.CheckImage = string.IsNullOrEmpty(str) ? false : Convert.ToBoolean(str);
            }
            if (this.Settings.CustomSettings.Keys.Contains(Keys.MODULE_CHECK_JAVASCRIPT))
            {
                var str = this.Settings.CustomSettings[Keys.MODULE_CHECK_JAVASCRIPT];
                this.CheckJavaScript = string.IsNullOrEmpty(str) ? false : Convert.ToBoolean(str);
            }
            if (this.Settings.CustomSettings.Keys.Contains(Keys.MODULE_CHECK_CSS))
            {
                var str = this.Settings.CustomSettings[Keys.MODULE_CHECK_CSS];
                this.CheckCss = string.IsNullOrEmpty(str) ? false : Convert.ToBoolean(str);
            }
            if (this.Settings.CustomSettings.Keys.Contains(Keys.MODULE_MAX_TIMETAKEN))
            {
                var str = this.Settings.CustomSettings[Keys.MODULE_MAX_TIMETAKEN];
                this.MaxTimeTaken = string.IsNullOrEmpty(str) ? 0 : Convert.ToDouble(str);
            }
        }
        public string ModuleName { get; set; }
        public string Version { get; set; }
        public string KoobooCMSVersion { get; set; }
        [DisplayName("Crawl Max Depth")]
        public int MaxDepth { get; set; }
        [DisplayName("Crawl Entry Url")]
        [Description("Crawl entry url, within \"http://\" or \"https://\".")]
        public string EntryUrl { get; set; }
        [DisplayName("Check image")]
        [Description("Enable Check image.")]
        public bool CheckImage { get; set; }
        [DisplayName("Check javascript")]
        [Description("Enable Check javascript.")]
        public bool CheckJavaScript { get; set; }
        [DisplayName("Check Css")]
        [Description("Enable Check Css.")]
        public bool CheckCss { get; set; }
        [DisplayName("Max time-taken")]
        [Description("Set a value, when the Http request time exceeds this value, the record will be highlighted.(Unit:second)")]
        public double MaxTimeTaken { get; set; }
        public ModuleSettings Settings { get; set; }
    }

    public class ThemesDatasource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var moduleName = Kooboo.Web.Mvc.AreaHelpers.GetAreaName(requestContext.RouteData);
            return Kooboo.CMS.Sites.Services.ServiceFactory.ModuleManager.AllThemes(moduleName).Select(it => new SelectListItem()
            {
                Text = it,
                Value = it
            });
        }
    }
    public class ModuleSettings_Metadata
    {
        [DataSource(typeof(ThemesDatasource))]
        [UIHint("DropDownList")]
        public string ThemeName { get; set; }

        public Entry Entry { get; set; }

        public Dictionary<string, string> CustomSettings { get; set; }
    }

}