using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.Globalization;
using Kooboo.CMS.Content.Query;

namespace Leon.Modules.Management.Controllers
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Web.Areas.Contents.Controllers.TextContentController), Order = 100)]
    public class TextContentController : Kooboo.CMS.Toolkit.Modules.ModuleTextContentController
    {
        public TextContentController(TextContentManager textContentManager, TextFolderManager textFolderManager,
            WorkflowManager workflowManager, ITextContentBinder binder, ITextContentProvider textContentProvider)
            : base(textContentManager, textFolderManager, workflowManager, binder, textContentProvider)
        {
            
        }

        public override ActionResult Create(string folderName, string parentFolder)
        {
            ViewResult viewResult = base.Create(folderName, parentFolder) as ViewResult;

            if (viewResult != null)
            {
                var content = viewResult.Model as TextContent;
                if (content != null && !String.IsNullOrWhiteSpace(Request["SiteName"]))
                {
                    content.FolderName = Request.QueryString["FolderName"];
                    var metaSetting = ManagementContext.Current.MetaSettingService.GetMetaSettingByFolder(content.FolderName);
                    if (metaSetting != null)
                    {
                        if (content.ContainsKey("HtmlTitle"))
                        {
                            content["HtmlTitle"] = metaSetting.Title;
                        }

                        if (content.ContainsKey("HtmlMetaDescription"))
                        {
                            content["HtmlMetaDescription"] = metaSetting.Description;
                        }
                    }
                }
            }

            return viewResult;
        }

        #region RebroadcastAll
        [HttpPost]
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult RebroadcastAll(string folderName, string parentFolder, string[] folderArr)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();

                var allDoc = textFolder.CreateQuery();
                foreach (var doc in allDoc)
                {
                    Kooboo.CMS.Content.EventBus.Content.ContentEvent.Fire(ContentAction.Add, doc);
                }
                data.Messages = new string[] { "Rebroadcast all success!".Localize() };
                data.ReloadPage = false;
            });
            return Json(data);
        }
        #endregion
    }
}