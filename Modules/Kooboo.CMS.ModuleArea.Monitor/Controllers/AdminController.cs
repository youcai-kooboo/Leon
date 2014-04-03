using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.Globalization;

namespace Kooboo.CMS.ModuleArea.Monitor.Controllers
{
    [ModuleContextInitialize]
    public class AdminController : Kooboo.CMS.ModuleArea.Monitor.AdminControllerBase
    {
        public ActionResult Index(string siteName)
        {
            ModuleInfo_Metadata moduleInfo = new ModuleInfo_Metadata(ModuleName, siteName);
            return View(moduleInfo);
        }

        [HttpPost]
        public ActionResult Index(string siteName, ModuleInfo_Metadata moduleInfo,int maxDepth,string entryUrl,bool checkImage,bool checkJavaScript,bool checkCss,double maxTimeTaken)
        {
            JsonResultData resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                try
                {
                    moduleInfo.Settings.CustomSettings.Add(Keys.MODULE_CRAWL_DEPTH, maxDepth.ToString());
                    moduleInfo.Settings.CustomSettings.Add(Keys.MODULE_CRAWL_ENTRY, entryUrl);
                    moduleInfo.Settings.CustomSettings.Add(Keys.MODULE_CHECK_IMAGE, checkImage.ToString());
                    moduleInfo.Settings.CustomSettings.Add(Keys.MODULE_CHECK_JAVASCRIPT, checkJavaScript.ToString());
                    moduleInfo.Settings.CustomSettings.Add(Keys.MODULE_CHECK_CSS, checkCss.ToString());
                    moduleInfo.Settings.CustomSettings.Add(Keys.MODULE_MAX_TIMETAKEN, maxTimeTaken.ToString());
                    var mi = new ModuleInfo();
                    mi.DefaultSettings = moduleInfo.Settings;
                    mi.ModuleName  = ModuleName;
                    ModuleContext.Current.SetModuleSettings(mi.DefaultSettings);
                    resultEntry.AddMessage("Module setting has been changed.".Localize());

                }
                catch (Exception e)
                {
                    resultEntry.AddException(e);
                }
            }
            return Json(resultEntry);
        }

        public ActionResult InitializeModule(string siteName)
        {
            JsonResultData resultEntry = new JsonResultData();
            if (string.IsNullOrEmpty(siteName))
            {
                resultEntry.AddErrorMessage("Parameter \"siteName\" is Null.");
            }
            
            resultEntry.AddMessage("Module initialize successful.");
            return Json(resultEntry);

            ////Create Repository
            //var currentRepository = this.RepositoryManager.Get(RepositoryName);
            //if (currentRepository == null)
            //{
            //    currentRepository = new Repository(RepositoryName);
            //    this.RepositoryManager.Add(currentRepository);
            //}
            
            ////Create Schemas
            //if (this.SchemaManager.Get(currentRepository, SchemaNames.Log) == null)
            //{
            //    var schema = new Schema();
            //    schema.Name = SchemaNames.Log;
            //    schema.IsDummy = false;
            //    schema.AddColumn(new Column() { Name = "DateTime", DataType = Common.DataType.DateTime, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "SiteName", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "ServerIP", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "Method", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "InOrOut", DataType = Common.DataType.Int, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "UriStem", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "UriQuery", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "Port", DataType = Common.DataType.Int, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "UserName", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "UserAgent", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "Referer", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "ClientIP", DataType = Common.DataType.String, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "Status", DataType = Common.DataType.Int, ShowInGrid = true });
            //    schema.AddColumn(new Column() { Name = "SubStatus", DataType = Common.DataType.Int, ShowInGrid = true });
            //    this.SchemaManager.Add(currentRepository, schema);
            //    this.SchemaManager.ResetForm(currentRepository, schema.Name, FormType.Grid | FormType.List);
            //}
            ////Create Folders
            //if (this.TextFolderManager.Get(currentRepository, FolderNames.Logs) == null)
            //{
            //    var textFolder = new TextFolder();
            //    textFolder.Name = FolderNames.Logs;
            //    textFolder.SchemaName = SchemaNames.Log;
            //    this.TextFolderManager.Add(currentRepository, textFolder);
            //}
        }

    }
}
