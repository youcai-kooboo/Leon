using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Toolkit;
using Leon.Modules.Management.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Leon.Modules.Management.Services
{
    public class PageMappingService : ServiceBase<PageMapping>
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.PageMapping;
            }
        }

        public override PageMapping Get(TextContent textContent)
        {
            return (textContent != null) ? new PageMapping(textContent) : null;
        }

        public PageMapping GetPageMapping(Site fromSite, string fromPageUrl, Site toSite)
        {
            return base.CreateQuery()
                .WhereEquals(PageMapping.FieldNames.FromSite, fromSite.FullName)
                .WhereEquals(PageMapping.FieldNames.FromPageUrl, fromPageUrl)
                .WhereEquals(PageMapping.FieldNames.ToSite, toSite.FullName)
                .Published()
                .FirstOrDefault()
                .MapTo<PageMapping>();
        }

        public string GetToPageUrl(Site toSite)
        {
            var fromSite = Site.Current;
            var fromPage = PageUtility.CurrentPage;
            var fromPageUrl = PageUtility.CurrentPageUrl;
            return GetToPageUrl(fromSite, fromPage, fromPageUrl, toSite);
        }

        public string GetToPageUrl(Site fromSite, Page fromPage, string fromPageUrl, string toSiteFullName)
        {
            Site toSite = new Site(toSiteFullName);
            toSite = toSite.AsActual();
            return GetToPageUrl(fromSite, fromPage, fromPageUrl, toSite);
        }

        public string GetToPageUrl(Site fromSite, Page fromPage, string fromPageUrl, Site toSite)
        {
            try
            {
                // when there are no route values, it throw exception.
                return GetToPageUrlInternal(fromSite, fromPage, fromPageUrl, toSite);
            }
            catch (Exception) { }

            return null;
        }

        private string GetToPageUrlInternal(Site fromSite, Page fromPage, string fromPageUrl, Site toSite)
        {
            string toPageUrl = String.Empty;

            var pageMapping = GetPageMapping(fromSite, fromPageUrl, toSite);
            if (pageMapping != null) // Custom page mapping
            {
                toPageUrl = PageUtility.GetAbsolutePageUrl(toSite, pageMapping.ToPageUrl);
            }
            else
            {
                var toPage = ServiceFactory.PageManager.Get(toSite, fromPage.FullName);
                if (toPage != null)
                {
                    var pageRequestContext = Page_Context.Current.PageRequestContext;
                    var viewData = pageRequestContext.ControllerContext.Controller.ViewData;
                    var fromDBContent = viewData["DBContent"] as TextContent;

                    RouteValueDictionary routeValues = null;
                    if (fromDBContent != null) // Dynamic page mapping
                    {
                        routeValues = pageRequestContext.RouteValues;

                        Repository toRepository = toSite.GetRepository();
                        TextFolder toFolder = null;
                        TextContent toDBContent = null;

                        Stack<string> routeKeys = new Stack<string>(routeValues.Keys);
                        while (routeKeys.Count > 0)
                        {
                            if (toDBContent == null)
                            {
                                toFolder = new TextFolder(toRepository, fromDBContent.FolderName);

                                string dbKey = fromDBContent.GetString(SystemFieldNames.DBKey);
                                if (!String.IsNullOrEmpty(dbKey)) // Try to find content by DBKey
                                {
                                    toDBContent = toFolder.CreateQuery().WhereEquals(SystemFieldNames.DBKey, dbKey).FirstOrDefault();
                                }

                                if (toDBContent == null) // Try to find content by OrginalUUID
                                {
                                    string orginalUUID = fromDBContent.OriginalUUID;
                                    if (!String.IsNullOrEmpty(orginalUUID))
                                    {
                                        toDBContent = toFolder.CreateQuery().WhereEquals(SystemFieldNames.OriginalUUID, orginalUUID).FirstOrDefault();
                                    }
                                }

                                if (toDBContent == null) // Didnot find anything
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (toFolder.Parent != null)
                                {
                                    TextFolder toFolderParent = toFolder.Parent as TextFolder;
                                    if (!string.IsNullOrEmpty(toFolderParent.SchemaName))
                                    {
                                        toFolder = toFolderParent;
                                    }
                                }

                                toDBContent = toDBContent.Parent(toFolder).FirstOrDefault();
                            }

                            string routeKey = routeKeys.Pop();
                            if (toDBContent != null)
                            {
                                routeValues[routeKey] = toDBContent[SystemFieldNames.UserKey];
                            }
                        }
                    }

                    toPageUrl = PageUtility.GetAbsolutePageUrl(toSite, toPage, routeValues);
                }
            }

            if (String.IsNullOrEmpty(toPageUrl))
            {
                toPageUrl = PageUtility.GetAbsolutePageUrl(toSite);
            }

            return toPageUrl.ToLower();
        }
    }
}