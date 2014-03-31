using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Toolkit;
using Leon.Modules.Management.Models;

namespace Leon.Modules.Management.Services
{
    /// <summary>
    /// Auto generated meta
    /// </summary>
    public class MetaSettingService : ServiceBase<MetaSetting>
    {
        private const string PageTitle = "{PageTitle}";

        public override string FolderName
        {
            get
            {
                return FolderNames.MetaSetting;
            }
        }

        public override IContentQuery<TextContent> CreateQuery()
        {
            return base.CreateQuery()
                .WhereEquals(Setting.FieldNames.Site, Site.Current.FullName);
        }

        public override MetaSetting Get(TextContent textContent)
        {
            if (textContent != null)
            {
                return new MetaSetting(textContent);
            }
            return null;
        }

        public MetaSetting GetMetaSettingByLayout(string layout)
        {
            return this.CreateQuery()
                .WhereEquals(MetaSetting.FieldNames.Layout, layout)
                .Published()
                .DefaultOrder()
                .FirstOrDefault()
                .MapTo<MetaSetting>();
        }

        public MetaSetting GetMetaSettingByFolder(string folder)
        {
            return this.CreateQuery()
                .WhereEquals(MetaSetting.FieldNames.Folder, folder)
                .Published()
                .DefaultOrder()
                .FirstOrDefault()
                .MapTo<MetaSetting>();
        }

        /// <summary>
        /// Reset values if they are empty
        /// </summary>
        public void Reset(string selectedUUIDs)
        {
            Reset(selectedUUIDs, resetOnlyIfValueIsEmpty: true);
        }

        /// <summary>
        /// Reset values
        /// </summary>
        /// <param name="resetOnlyIfValueIsEmpty">Reset only if value is empty</param>
        public void Reset(string selectedUUIDs, bool resetOnlyIfValueIsEmpty)
        {
            IEnumerable<MetaSetting> metaSettings;
            if (!String.IsNullOrEmpty(selectedUUIDs))
            {
                metaSettings = this.CreateQuery()
                   .WhereIn(SystemFieldNames.UUID, selectedUUIDs.SplitRemoveEmptyEntries(','))
                   .Published()
                   .DefaultOrder()
                   .MapTo<MetaSetting>();
            }
            else
            {
                metaSettings = Enumerable.Empty<MetaSetting>();
            }

            var site = Site.Current;
            var repository = new Repository(site.Repository);

            // Update folders
            var folderMetaSettings = metaSettings.Where(it => !String.IsNullOrEmpty(it.Folder)).ToList();
            if (folderMetaSettings.Count > 0)
            {
                foreach (var folderMetaSetting in folderMetaSettings)
                {
                    var folder = new TextFolder(repository, folderMetaSetting.Folder);
                    var contents = folder.CreateQuery().ToList();
                    foreach (var content in contents)
                    {
                        if (content.ContainsKey("HtmlTitle") || content.ContainsKey("HtmlMetaDescription"))
                        {
                            string pageTitle = PageTitle;
                            if (content.ContainsKey("BreadCrumb"))
                            {
                                pageTitle = content.GetString("BreadCrumb");
                            }
                            else if (content.ContainsKey("Name"))
                            {
                                pageTitle = content.GetString("Name");
                            }
                            else if (content.ContainsKey("Title"))
                            {
                                pageTitle = content.GetString("Title");
                            }

                            if (resetOnlyIfValueIsEmpty)
                            {
                                if (String.IsNullOrEmpty(content.GetString("HtmlTitle")) || String.IsNullOrEmpty(content.GetString("HtmlMetaDescription")))
                                {
                                    if (String.IsNullOrEmpty(content.GetString("HtmlTitle")))
                                    {
                                        content["HtmlTitle"] = folderMetaSetting.Title.Replace(PageTitle, pageTitle);
                                    }

                                    if (String.IsNullOrEmpty(content.GetString("HtmlMetaDescription")))
                                    {
                                        content["HtmlMetaDescription"] = folderMetaSetting.Description.Replace(PageTitle, pageTitle);
                                    }

                                    Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Update(
                                        repository,
                                        folder,
                                        content.UUID,
                                        content.ToNameValueCollection());
                                }
                            }
                            else
                            {
                                content["HtmlTitle"] = folderMetaSetting.Title.Replace(PageTitle, pageTitle);
                                content["HtmlMetaDescription"] = folderMetaSetting.Description.Replace(PageTitle, pageTitle);

                                Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Update(
                                    repository,
                                    folder,
                                    content.UUID,
                                    content.ToNameValueCollection());
                            }
                        }
                    }
                }
            }

            // Update pages
            var layoutMetaSettings = metaSettings.Where(it => !String.IsNullOrEmpty(it.Layout)).ToList();
            if (layoutMetaSettings.Count > 0)
            {
                var allPages = ServiceFactory.PageManager.All(site, String.Empty);
                RecursionVisitor.Visit(allPages, visit => pages =>
                {
                    if (pages.Count() > 0)
                    {
                        foreach (var page in pages)
                        {
                            if (page.IsLocalized(site))
                            {
                                var layoutMetaSetting = layoutMetaSettings.FirstOrDefault(it => it.Layout.Equals(page.Layout, StringComparison.OrdinalIgnoreCase));
                                if (layoutMetaSetting != null)
                                {
                                    string pageTitle = page.ContentTitle;
                                    if (resetOnlyIfValueIsEmpty)
                                    {
                                        if (String.IsNullOrEmpty(page.HtmlMeta.HtmlTitle) || String.IsNullOrEmpty(page.HtmlMeta.Description))
                                        {
                                            if (String.IsNullOrEmpty(page.HtmlMeta.HtmlTitle))
                                            {
                                                page.HtmlMeta.HtmlTitle = layoutMetaSetting.Title.Replace(PageTitle, pageTitle);
                                            }

                                            if (String.IsNullOrEmpty(page.HtmlMeta.Description))
                                            {
                                                page.HtmlMeta.Description = layoutMetaSetting.Description.Replace(PageTitle, pageTitle);
                                            }

                                            ServiceFactory.PageManager.Update(site, page, page);
                                        }
                                    }
                                    else
                                    {
                                        page.HtmlMeta.HtmlTitle = layoutMetaSetting.Title.Replace(PageTitle, pageTitle);
                                        page.HtmlMeta.Description = layoutMetaSetting.Description.Replace(PageTitle, pageTitle);

                                        ServiceFactory.PageManager.Update(site, page, page);
                                    }
                                }
                            }

                            var childPages = ServiceFactory.PageManager.ChildPages(site, page.FullName, String.Empty);
                            visit(childPages);
                        }
                    }
                });
            }
        }
    }
}