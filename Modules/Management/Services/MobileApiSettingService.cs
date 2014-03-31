using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services {

    public class MobileApiSettingService : SettingService {

        public MobileApiSettingService() {
        }

        public override string FolderName {
            get {
                return FolderNames.MobileApiSetting;
            }
        }

        public string GetHost() {
            return GetSettingValue("Host", "http://api.budgetair.nl");
        }

        public string GetLanguageId() {
            return GetSettingValue("LanguageId", "NL");
        }

        public bool UseProxy() {
            return GetSettingValue("UseProxy", "false") == "true";
        }
    }
}
