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
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.ComponentModel;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.ModuleArea.Monitor
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 1)]
    public class AdminControllerBase : ModuleAreaControllerBase
    {
        public string RepositoryName
        {
            get
            {
                return MonitorAreaRegistration.ModuleName;
            }
        }       

        static AdminControllerBase()
        {
            TypeDescriptorHelper.RegisterMetadataType(typeof(ModuleSettings), typeof(ModuleSettings_Metadata));
        }

        [NonAction]
        public int GetMaxDepth()
        {
            int depth = this.ModuleSetting.MaxDepth;
            if (depth <= 0)
            {
                depth = 3;
            }
            return depth;
        }

        [NonAction]
        public string GetEntryUrl()
        {
            string entryUrl = this.ModuleSetting.EntryUrl;
            if (string.IsNullOrEmpty(entryUrl) && Site.Current.Domains != null && Site.Current.Domains.Length > 0)
            {
                entryUrl = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(Site.Current.Domains[0], " ");
            }
            return entryUrl;
        }

        [NonAction]
        public double GetMaxTimeTaken()
        {
            return this.ModuleSetting.MaxTimeTaken;
        }

        [NonAction]
        public bool GetCheckImage()
        {
            return this.ModuleSetting.CheckImage;
        }

        [NonAction]
        public bool GetCheckJs()
        {
            return this.ModuleSetting.CheckJavaScript;
        }

        [NonAction]
        public bool GetCheckCss()
        {
            return this.ModuleSetting.CheckCss;
        }

        private ModuleInfo_Metadata _moduleSetting;
        protected ModuleInfo_Metadata ModuleSetting
        {
            get
            {
                if (this._moduleSetting == null)
                {
                    this._moduleSetting = new ModuleInfo_Metadata(MonitorAreaRegistration.ModuleName, Sites.Models.Site.Current.Name);
                }
                return this._moduleSetting;
            }
        }
    }
}
