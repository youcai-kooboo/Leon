using System;
using System.Linq;
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
                    .Where(m => m.Published == true)
                    .FirstOrDefault()
                    .MapTo<IPSetting>();
            }
            catch (Exception ex)
            {
                _ipSetting = null;
            }

            return _ipSetting;
        }

    }
}
