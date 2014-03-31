using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Toolkit.Templates;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Services {

    public class GameOmnitureSettingService : SettingService {

        public override string FolderName {
            get {
                return FolderNames.OmnitureSetting;
            }
        }

        public GameOmnitureSettingService() {
            this.Initialize();
        }

        public void Initialize() {
            GetGameOpenSetting();
            GetGameSendMailSetting();
            GetGameReceivedConfirmationMailSettiing();
            GetGamePlayFirstTimeSetting();
            GetGameLostFirstAttemptSetting();
            GetGamePlaySecondTimeSetting();
            GetGameOverSetting();
            GetGameWinningSetting();
        }

        public string GetGameSetting(string key, string strDefault = null) {
            var omniture = GetSettingValue(key, strDefault ?? "<!-- Omniture code -->");
            if (!string.IsNullOrEmpty(omniture)) {
                var templateRender = new TemplateRender(omniture);
                templateRender.Text("PageName", PageUtility.CurrentPageUrl);
                omniture = templateRender.ToString();
            }
            return omniture;
        }

        public string GetGameOpenSetting() {
            return GetGameSetting("GameOpenOmnitureCode");
        }

        public string GetGameSendMailSetting() {
            return GetGameSetting("GameSendMailOmnitureCode");
        }

        public string GetGameReceivedConfirmationMailSettiing() {
            return GetGameSetting("GameReceivedConfirmationMailOmnitureCode");
        }

        public string GetGamePlayFirstTimeSetting() {
            return GetGameSetting("GamePlayFirstTimeOmnitureCode");
        }

        public string GetGameLostFirstAttemptSetting() {
            return GetGameSetting("GameLostFirstAttemptOmnitureCode");
        }

        public string GetGamePlaySecondTimeSetting() {
            return GetGameSetting("GamePlaySecondTimeOmnitureCode");
        }

        public string GetGameWinningSetting() {
            return GetGameSetting("GameWinningOmnitureCode");
        }

        public string GetGameOverSetting() {
            return GetGameSetting("GameOverOmnitureCode");
        }
    }
}
