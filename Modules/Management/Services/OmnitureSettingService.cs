using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Toolkit;
using Kooboo.CMS.Toolkit.Templates;
//using RazorEngine;
//using RazorEngine.Templating;

namespace Leon.Modules.Management.Services
{
    //[Serializable]
    //public class OmnitureTemplateModel
    //{
    //    public string PageName
    //    {
    //        get;
    //        set;
    //    }
    //}

    public class OmnitureSettingService : SettingService
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.OmnitureSetting;
            }
        }

        /*
        /// <summary>
        /// RazorEngine V3
        /// https://github.com/Antaris/RazorEngine
        /// 
        /// IsolatedTemplateService instances do not support anonymous or dynamic types.
        /// </summary>
        /// <returns></returns>
        public string GetOmniture()
        {
            string omniture = GetSettingValue("Omniture", "<!-- Omniture code -->");
            if(!String.IsNullOrEmpty(omniture))
            {
                using(var templateService = new IsolatedTemplateService())
                {
                    var model = new OmnitureTemplateModel { PageName = PageUtility.CurrentPageUrl };
                    omniture = templateService.Parse(omniture, model, null, "OmnitureTemplate");
                }
            }
            return omniture;
        }*/

        public string GetOmniture()
        {
            string omniture = GetSettingValue("Omniture", "<!-- Omniture code -->");
            if(!String.IsNullOrEmpty(omniture))
            {
                var templateRender = new TemplateRender(omniture);
                templateRender.Text("PageName", PageUtility.CurrentPageUrl);
                omniture = templateRender.ToString();
            }
            return omniture;
        }

        public string GetSearchAnalysis()
        {
            return GetSettingValue("SearchAnalysis");
        }
    }
}