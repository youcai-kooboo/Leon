#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.ModuleArea.Monitor.Repositories.Default;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleEvents), Key = MonitorAreaRegistration.ModuleName)]
    public class DefaultModuleAction : IModuleEvents
    {       
        public void OnInstalling(ModuleContext moduleContext, System.Web.Mvc.ControllerContext controllerContext)
        {
            
        }

        public void OnUninstalling(ModuleContext moduleContext, System.Web.Mvc.ControllerContext controllerContext)
        {
            
        }

        public void OnExcluded(ModuleContext moduleContext)
        {
            string filePath = DatabaseFactory.GetFilePath(moduleContext.Site.FullName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void OnIncluded(ModuleContext moduleContext)
        {
            
        }

        public void OnReinstalling(ModuleContext moduleContext, System.Web.Mvc.ControllerContext controllerContext, Sites.Extension.ModuleArea.Management.InstallationContext installationContext)
        {
            
        }
    }
}
