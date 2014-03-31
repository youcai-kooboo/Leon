using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Toolkit;
using Leon.Modules.Management;

namespace Leon.Business.Controllers
{
    public class SyncController : Controller
    {
        [HttpPost]
        public JsonResult SyncFromParentSite(string moduleRepositoryName, string siteName, string folderName, string @return)
        {
            var currentSite = SiteHelper.Parse(siteName);
            var jsonResultData = new JsonResultData();
 
            if (currentSite != null)
            {
                var parentSite = currentSite.Parent.AsActual();
                if (parentSite != null)
                {
                    if (!currentSite.FullName.Equals(parentSite.FullName, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            switch (folderName)
                            {
                                case Modules.Management.FolderNames.PixelPerPage:
                                    SyncPixelPage(currentSite, parentSite);
                                    break;
                                case Modules.Management.FolderNames.PixelPerLayout:
                                    SyncPixelLayout(currentSite, parentSite);
                                    break;
                                default:
                                    SyncSettings(currentSite, parentSite, folderName);
                                    break;
                            }

                            jsonResultData.RedirectUrl = @return;
                            jsonResultData.AddMessage(SuccessMessage);
                        }
                        catch (Exception ex)
                        {
                            jsonResultData.Success = false;
                            jsonResultData.AddErrorMessage(ex.Message);
                        }

                    }
                }
            }

            return Json(jsonResultData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SyncToChildrenSites(string moduleRepositoryName, string siteName, string folderName, string[] docs, string @return)
        {
            var currentSite = SiteHelper.Parse(siteName);
            var jsonResultData = new JsonResultData();

            if (currentSite != null)
            {
                try
                {
                    SyncDatas(currentSite, folderName, docs);

                    jsonResultData.AddMessage(SuccessMessage);
                }
                catch (Exception ex)
                {
                    jsonResultData.Success = false;
                    jsonResultData.AddErrorMessage(ex.Message);
                }
            }

            return Json(jsonResultData, JsonRequestBehavior.AllowGet);
        }

        #region Private Methods 

        private const string SuccessMessage = "Synchronize datas successfully.";
  

        private void SyncDatas(Site currentSite, string folderName, string[] uuids = null)
        {
            var childrenSites = ServiceFactory.SiteManager.ChildSites(currentSite);
            if (childrenSites.Any())
            {
                string[] uuidsChildren = null;
                foreach (var childrenSite in childrenSites)
                {
                    switch (folderName)
                    {
                        case Modules.Management.FolderNames.PixelPerPage:
                            uuidsChildren = SyncPixelPage(childrenSite, currentSite, uuids);
                            break;
                        case Modules.Management.FolderNames.PixelPerLayout:
                            uuidsChildren = SyncPixelLayout(childrenSite, currentSite, uuids);
                            break;
                        default:
                            uuidsChildren = SyncSettings(childrenSite, currentSite, folderName, uuids);
                            break;
                    }

                    SyncDatas(childrenSite, folderName, uuidsChildren);
                }
            }
        }

        private string[] SyncPixelPage(Site currentSite, Site parentSite, string[] uuids = null)
        {
            var docsChildren = new List<string>();
            var service = ManagementContext.Current.PixelPerPageService;
            var parentSiteSettings = service.GetAllBySiteName(parentSite.FullName);
            if (uuids != null)
            {
                parentSiteSettings = parentSiteSettings.Where(m => uuids.Contains(m.UUID));
            }
            var currentSiteSettings = service.GetAllBySiteName(currentSite.FullName);

            foreach (var parentSiteSetting in parentSiteSettings)
            {
                var currentSiteSetting = currentSiteSettings.FirstOrDefault(it =>
                    it.PageName.Equals(parentSiteSetting.PageName, StringComparison.OrdinalIgnoreCase));

                if (currentSiteSetting == null)
                {
                    currentSiteSetting = parentSiteSetting;
                    currentSiteSetting.UUID = UUIDGenerator.Generate();
                    currentSiteSetting.Site = currentSite.FullName;

                    var folder = Kooboo.CMS.Content.Models.ContentExtensions.GetFolder(currentSiteSetting);
                    folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);
                    service.Add(currentSiteSetting, folder);
                }
                else
                {
                    currentSiteSetting.DisplayPosition = parentSiteSetting.DisplayPosition;
                    currentSiteSetting.StartDate = parentSiteSetting.StartDate;
                    currentSiteSetting.StopDate = parentSiteSetting.StopDate;
                    currentSiteSetting.Description = parentSiteSetting.Description;
                    currentSiteSetting.Code = parentSiteSetting.Code;

                    var folder = Kooboo.CMS.Content.Models.ContentExtensions.GetFolder(currentSiteSetting);
                    folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);
                    service.Update(currentSiteSetting, folder);
                }

                docsChildren.Add(currentSiteSetting.UUID);
            }

            return docsChildren.ToArray();
        }

        private string[] SyncPixelLayout(Site currentSite, Site parentSite, string[] uuids = null)
        {
            var docsChildren = new List<string>();
            var service = ManagementContext.Current.PixelPerLayoutService;
            var parentSiteSettings = service.GetAllBySiteName(parentSite.FullName);
            if (uuids != null)
            {
                parentSiteSettings = parentSiteSettings.Where(m => uuids.Contains(m.UUID));
            }
            var currentSiteSettings = service.GetAllBySiteName(currentSite.FullName);

            foreach (var parentSiteSetting in parentSiteSettings)
            {
                var currentSiteSetting = currentSiteSettings.FirstOrDefault(it =>
                    it.Layout.Equals(parentSiteSetting.Layout, StringComparison.OrdinalIgnoreCase));

                if (currentSiteSetting == null)
                {
                    currentSiteSetting = parentSiteSetting;
                    currentSiteSetting.UUID = UUIDGenerator.Generate();
                    currentSiteSetting.Site = currentSite.FullName;

                    var folder = Kooboo.CMS.Content.Models.ContentExtensions.GetFolder(currentSiteSetting);
                    folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);
                    service.Add(currentSiteSetting, folder);
                }
                else
                {
                    currentSiteSetting.DisplayPosition = parentSiteSetting.DisplayPosition;
                    currentSiteSetting.StartDate = parentSiteSetting.StartDate;
                    currentSiteSetting.StopDate = parentSiteSetting.StopDate;
                    currentSiteSetting.Description = parentSiteSetting.Description;
                    currentSiteSetting.Code = parentSiteSetting.Code;

                    var folder = Kooboo.CMS.Content.Models.ContentExtensions.GetFolder(currentSiteSetting);
                    folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);
                    service.Update(currentSiteSetting, folder);
                }

                docsChildren.Add(currentSiteSetting.UUID);
            }

            return docsChildren.ToArray();
        }

        private string[] SyncSettings(Site currentSite, Site parentSite, string folderName, string[] uuids = null)
        {
            var docsChildren = new List<string>();
            var service = ManagementContext.Current.SettingService;
            var parentSiteSettings = service.GetSettings(parentSite.FullName).Where(m => m.FolderName == folderName);
            if (uuids != null)
            {
                parentSiteSettings = parentSiteSettings.ToList().Where(m => uuids.Contains(m.UUID));
            }
            var currentSiteSettings = service.GetSettings(currentSite.FullName).Where(m => m.FolderName == folderName);

            foreach (var parentSiteSetting in parentSiteSettings)
            {
                var currentSiteSetting = currentSiteSettings.FirstOrDefault(it =>
                    it.Key.Equals(parentSiteSetting.Key, StringComparison.OrdinalIgnoreCase));

                if (currentSiteSetting == null)
                {
                    currentSiteSetting = parentSiteSetting;
                    currentSiteSetting.UUID = UUIDGenerator.Generate();
                    currentSiteSetting.Site = currentSite.FullName;

                    var folder = Kooboo.CMS.Content.Models.ContentExtensions.GetFolder(currentSiteSetting);
                    folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);
                    service.Add(currentSiteSetting, folder);
                }
                else
                {
                    currentSiteSetting.Value = parentSiteSetting.Value;
                    var folder = Kooboo.CMS.Content.Models.ContentExtensions.GetFolder(currentSiteSetting);
                    folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);
                    service.Update(currentSiteSetting, folder);
                }

                docsChildren.Add(currentSiteSetting.UUID);
            }

             

            return docsChildren.ToArray();
        } 
        #endregion
    }
}
