using System;
using System.Linq;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Toolkit;
using Kooboo.CMS.Toolkit.Services;
using Leon.Modules.IPFilter.Models;

namespace Leon.Modules.IPFilter.Services
{
    public class IPSettingService : ServiceBase<IPSetting>
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.IPSetting;
            }
        }

        public IPSetting GetSettings()
        {
            var _ipSetting = new IPSetting();

            try
            {
                _ipSetting = base.CreateQuery()
                    .FirstOrDefault()
                    .MapTo<IPSetting>();
            }
            catch (Exception ex)
            {
                _ipSetting = null;
            }

            return _ipSetting;
        }

        public void Save(IPSetting settings)
        {
            var _ipSetting = GetSettings();

            if (_ipSetting != null)
            {
                _ipSetting.Published = settings.Published;
                _ipSetting.FilterType = settings.FilterType;
                _ipSetting.FilterScope = settings.FilterScope;
                _ipSetting.ForbiddenHtml = settings.ForbiddenHtml;

                Update(_ipSetting);
            }
            else
            {
                Add(settings);  
            }
        }

    }
}
