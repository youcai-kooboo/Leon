using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Search;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Leon.KB.Extensions;

namespace Leon.Business.Jobs
{
    public class PageSearchJob : CustomJobBase
    {
        public override void Execute(object executionState)
        {
            Thread.Sleep(1000*60*5);
            var rootSites = ServiceFactory.SiteManager.AllRootSites();
            foreach (var rootSite in rootSites)
            {
                ScanSite(rootSite);
            }

            this.Stop();
        }

        private void ScanSite(Site site)
        {
            var pages = ServiceFactory.PageManager.All(site, String.Empty);
            foreach (var page in pages)
            {
                ScanChildrenPages(page, site);
            }

            var childSites = ServiceFactory.SiteManager.ChildSites(site);
            if (childSites.Any())
            {
                foreach (var childSite in childSites)
                {
                    ScanSite(childSite);
                }
            }
        }

        private  void ScanChildrenPages(Page page, Site site)
        {
            if (page.Searchable)
            {
                var repository = new Repository(site.AsActual().Repository);
                SearchHelper.OpenService(repository).Add(page);
                SearchHelper.OpenService(repository).Update(page);
            }
            var subPages = ServiceFactory.PageManager.ChildPages(site, page.FullName, "");
            if (subPages.Any())
            {
                foreach (var item in subPages)
                {
                    ScanChildrenPages(item, site);
                }
            }
        }

    }
}
