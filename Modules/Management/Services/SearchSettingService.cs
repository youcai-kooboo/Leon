using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services
{
    public class SearchSettingService : SettingService
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.SearchSetting;
            }
        }
    }
}