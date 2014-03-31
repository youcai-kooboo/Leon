using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leon.Modules.Management.Services {

    public class TagPlacementService : SettingService {

        public override string FolderName {
            get {
                return FolderNames.TagPlacementSetting;
            }
        }

        public string GetCode() {
            return this.GetSettingValue("Code", string.Empty);
        }
    }
}
