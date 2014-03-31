using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Toolkit;
using Leon.Modules.Management.Models;

namespace Leon.Modules.Management.Services
{
    public class HtmlBannerService : ServiceBase<HtmlBanner>
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.HtmlBanner;
            }
        }

        
        public override IContentQuery<TextContent> CreateQuery()
        {
            return base.CreateQuery()
                .WhereEquals("Site", Site.Current.FullName);
        }

        public override HtmlBanner Get(TextContent textContent)
        {
            if(textContent != null)
            {
                return new HtmlBanner(textContent);
            }
            return null;
        }

        public HtmlBanner GetHtmlBanner(string size)
        {
            return Get(HtmlBanner.FieldNames.Size, size);
        }

        #region Copy

        public void BrocastOnRoot() {
            if (Site.Current.RootSite() == Site.Current) {
                var rootBanners = CreateBroadcastQuery(Site.Current.RootSite().FullName).MapTo<HtmlBanner>();
                Brocast(Site.Current, rootBanners);
            }
        }

        public void CopyFromRoot() {
            if (Site.Current.RootSite() != Site.Current) {
                var rootBanners = CreateBroadcastQuery(Site.Current.RootSite().FullName).MapTo<HtmlBanner>();
                CopyTo(Site.Current, rootBanners);
            }
        }

        private void Brocast(Site site, IEnumerable<HtmlBanner> banners) {
            var childrenSites = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.ChildSites(site);
            foreach (var itemSite in childrenSites) {
                CopyTo(itemSite, banners);
                Brocast(itemSite, banners);
            }
        }

        private void CopyTo(Site site, IEnumerable<HtmlBanner> banners) {
            foreach (var item in banners) {
                var exist = CreateBroadcastQuery(site.FullName).WhereEquals(HtmlBanner.FieldNames.Size, item.Size).Count() > 0;
                if (!exist) {
                    string logo = null;
                    if (!string.IsNullOrEmpty(item.Logo)) {
                        logo = item.Logo.Replace("Contents/Yarden/Media", string.Format("Contents/{0}/Media", site.Repository));
                    }
                    string image = null;
                    if (!string.IsNullOrEmpty(item.Image)) {
                        image = item.Image.Replace("Contents/Yarden/Media", string.Format("Contents/{0}/Media", site.Repository));
                    }
                    string slogan = null;
                    if (!string.IsNullOrEmpty(item.Slogan)) {
                        slogan = item.Slogan.Replace("Contents/Yarden/Media", string.Format("Contents/{0}/Media", site.Repository));
                    }
                    ManagementContext.Current.HtmlBannerService.Add(new HtmlBanner() {
                        Site = site.FullName,
                        Size = item.Size,
                        Logo = logo,
                        Image = image,
                        Slogan = slogan
                    });
                }
            }
        }
        
        private IContentQuery<TextContent> CreateBroadcastQuery(string siteFullName) {
            return base.CreateQuery().WhereEquals("Site", siteFullName);
        }
         

        #endregion
    }
}