using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Toolkit;
using Leon.Modules.Management.Services;

namespace Leon.Modules.Management
{
    public class ManagementContext : ContextBase
    {
        public static ManagementContext Current
        {
            get
            {
                return ContextUtility.GetOrAdd<ManagementContext>("ManagementContext", () => new ManagementContext());
            }
        }

        public SettingService SettingService
        {
            get
            {
                return this.GetService<SettingService>();
            }
        }

        public PageMappingService PageMappingService
        {
            get
            {
                return this.GetService<PageMappingService>();
            }
        }

        public PixelPerLayoutService PixelPerLayoutService
        {
            get
            {
                return this.GetService<PixelPerLayoutService>();
            }
        }

        public PixelPerPageService PixelPerPageService
        {
            get
            {
                return this.GetService<PixelPerPageService>();
            }
        }

        public SiteScriptSettingService BannerScriptSettingService
        {
            get
            {
                return this.GetService<SiteScriptSettingService>();
            }
        }

        public LayerService LayerService
        {
            get
            {
                return this.GetService<LayerService>();
            }
        }

        public GoogleAnalyticsSettingService GoogleAnalyticsSettingService
        {
            get
            {
                return this.GetService<GoogleAnalyticsSettingService>();
            }
        }

        public MobileRedirectSettingService MobileRedirectSettingService
        {
            get
            {
                return this.GetService<MobileRedirectSettingService>();
            }
        }

        public IPadRedirectSettingService IPadRedirectSettingService
        {
            get
            {
                return this.GetService<IPadRedirectSettingService>();
            }
        }

        public RedirectBasedOnBrowserLanguageService RedirectBasedOnBrowserLanguageService
        {
            get
            {
                return this.GetService<RedirectBasedOnBrowserLanguageService>();
            }
        }
 

        public OmnitureSettingService OmnitureSettingService
        {
            get
            {
                return this.GetService<OmnitureSettingService>();
            }
        }

        public SearchSettingService SearchSettingService
        {
            get
            {
                return this.GetService<SearchSettingService>();
            }
        }
 
        public HtmlBannerService HtmlBannerService
        {
            get
            {
                return this.GetService<HtmlBannerService>();
            }
        }

        public SocialRibbonSettingService SocialRibbonSettingService
        {
            get
            {
                return this.GetService<SocialRibbonSettingService>();
            }
        }

        public HermitTermsService HermitTermsService
        {
            get
            {
                return this.GetService<HermitTermsService>();
            }
        }

        public MetaSettingService MetaSettingService
        {
            get
            {
                return this.GetService<MetaSettingService>();
            }
        }

        public GameOmnitureSettingService GameOmnitureSettingService {
            get {
                return this.GetService<GameOmnitureSettingService>();
            }
        }
         
        public TopBannerService TopBannerService
        {
            get
            {
                return this.GetService<TopBannerService>();
            }
        }

        public CheckoutApiSettingService CheckoutApiSettingService
        {
            get
            {
                return this.GetService<CheckoutApiSettingService>();
            }
        }

        public CheckoutSiteSettingService CheckoutSiteSettingService
        {
            get
            {
                return this.GetService<CheckoutSiteSettingService>();
            }
        }

        public TagPlacementService TagPlacementService {
            get {
                return this.GetService<TagPlacementService>();
            }
        }

        public CommonSettingService CommonSettingService()
        {
            return this.GetService<CommonSettingService>();
        }

        public ApiSettingService ApiSettingService()
        {
            return this.GetService<ApiSettingService>();
        }

        #region Mobile Related

        public MobileApiSettingService MobileApiSettingService {
            get {
                return this.GetService<MobileApiSettingService>();
            }
        }

        public MobileLanguageService MobileLanguageService {
            get {
                return this.GetService<MobileLanguageService>();
            }
        }

        public MobileSiteSettingService MobileSiteSettingService {
            get {
                return this.GetService<MobileSiteSettingService>();
            }
        }
        #endregion
    }
}