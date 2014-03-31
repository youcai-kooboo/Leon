using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Services
{
    public class HermitTermsService : SettingService
    {
        private bool _enable;
        private string _textContent;
        private string _buttonText;

        public override string FolderName
        {
            get
            {
                return FolderNames.HermitTermsSetting;
            }
        }

        public HermitTermsService()
        {
            _enable = GetSettingValue("Enable", "false").AsBool();
            _textContent = GetSettingValue("TextContent", "TextContent");
            _buttonText = GetSettingValue("ButtonText", "ButtonText");
        }

        /// <summary>
        /// Hermit terms enable/disable
        /// </summary>
        /// <returns></returns>
        public bool GetEnable()
        {
            return _enable;
        }

        /// <summary>
        /// Hermit terms text content
        /// </summary>
        /// <returns></returns>
        public string GetTextContent()
        {
            return _textContent;
        }

        /// <summary>
        /// Hermit terms button text
        /// </summary>
        /// <returns></returns>
        public string GetButtonText()
        {
            return _buttonText;
        }
    }
}