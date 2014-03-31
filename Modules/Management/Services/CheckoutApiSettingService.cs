using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services {

    public class CheckoutApiSettingService : MobileApiSettingService {

        public override string FolderName {
            get {
                return FolderNames.CheckoutApiSetting;
            }
        }
    }
}
