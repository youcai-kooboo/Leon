using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Globalization;
using Leon.Modules.IPFilter.Models;
using Leon.Modules.IPFilter.Services;

namespace Leon.Modules.IPFilter.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization]
    public class SettingsController : AreaControllerBase
    {
        private IPSettingService _settingService;

        public SettingsController()
        {
            var request = System.Web.HttpContext.Current.Request;
            Site.Current = new Site(request.QueryString["siteName"]);
            Repository.Current = new Repository(Site.Current.AsActual().Repository);

            _settingService = IPFilterContext.Current.IPSettingService;
        }

        public ActionResult Index()
        {
            var model = new SettingModel();
            var settings = _settingService.GetSettings();

            int filterType = -1;
            int filterScope = -1;

            if (settings == null)
            {
                settings = new IPSetting()
                {
                    Published = false
                };
            }
            else
            {
                filterType = settings.FilterType;
                filterScope = settings.FilterScope;
            }

            model.Enable = settings.Published ?? false;
            model.ForbiddenHtml = settings.ForbiddenHtml;
            model.FilterType = filterType;
            model.FilterScope = filterScope;
            model.FilterTypeList = GetFilterTypeList(filterType);
            model.FilterScopeList = GetFilterScopeList(filterScope);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SettingModel settingModel, string @return)
        {
            var result = new JsonResultData();

            if (ModelState.IsValid)
            {
                result.RunWithTry(data =>
                {
                    var settings = _settingService.GetSettings();
                    if (settings == null)
                    {
                        settings = new IPSetting()
                        {
                            Published = false
                        };
                    }

                    settings.Published = settingModel.Enable;
                    settings.ForbiddenHtml = settingModel.ForbiddenHtml;
                    settings.FilterType = settingModel.FilterType;
                    settings.FilterScope = settingModel.FilterScope;

                    _settingService.Save(settings);

                    settingModel.FilterTypeList = GetFilterTypeList(settingModel.FilterType);
                    settingModel.FilterScopeList = GetFilterScopeList(settingModel.FilterScope);

                    data.RedirectUrl = @return;
                    data.Success = true;
                    data.AddMessage("Setting has been changed.".Localize());
                });
            }

            return Json(result);
        }

        private IEnumerable<SelectListItem> GetFilterTypeList(int filterType = -1)
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem()
            {
                Text = FilterType.Whitelist.ToString(),
                Value = ((int) FilterType.Whitelist).ToString(),
                Selected = (int) FilterType.Whitelist == filterType
            });
            items.Add(new SelectListItem()
            {
                Text = FilterType.Blacklist.ToString(),
                Value = ((int) FilterType.Blacklist).ToString(),
                Selected = (int) FilterType.Blacklist == filterType
            });

            return items;
        }

        private IEnumerable<SelectListItem> GetFilterScopeList(int filterScope = -1)
        {
            var items = new List<SelectListItem>();

            if (IsFirstRootSite())
            {
                items.Add(new SelectListItem()
                {
                    Text = Models.FilterScope.All.ToString(),
                    Value = ((int) Models.FilterScope.All).ToString(),
                    Selected = (int) Models.FilterScope.All == filterScope
                });
                items.Add(new SelectListItem()
                {
                    Text = Models.FilterScope.Backend.ToString(),
                    Value = ((int) Models.FilterScope.Backend).ToString(),
                    Selected = (int) Models.FilterScope.Backend == filterScope
                });
            }

            items.Add(new SelectListItem()
            {
                Text = Models.FilterScope.Frontend.ToString(),
                Value = ((int) Models.FilterScope.Frontend).ToString(),
                Selected = (int) Models.FilterScope.Frontend == filterScope
            });

            return items;
        }

        private bool IsFirstRootSite()
        {
            return Site.Current == Providers.SiteProvider.AllRootSites().FirstOrDefault();
        }
    }
}
