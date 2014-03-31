using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Services
{
    public class SocialRibbonSettingService : SettingService
    {
        public SocialRibbonSettingService()
        {
            _newsletterConnectionServerAddress = GetSettingValue("NewsletterConnectionServerAddress",
                                                                 "http://p5apic.emv2.com/apimember/services/MemberService");
            _newsletterConnectionKey = GetSettingValue("NewsletterConnectionKey",
                                                       "The manager key copied from Campaign Commander Email & Mobile Edition");
            _newsletterConnectionLogin = GetSettingValue("NewsletterConnectionLogin",
                                                         "The login provider for API access");
            _newsletterConnectionPassword = GetSettingValue("NewsletterConnectionPassword", "The password");
            _newsletterConnectionLanguageID = GetSettingValue("NewsletterConnectionLanguageID", "");
            _newsletterConnectionEmailStatus = GetSettingValue("NewsletterConnectionEmailStatus", "1");
            _newsletterConnectionHttpCallAddress = GetSettingValue("NewsletterConnectionHttpCallAddress",
                                                                   "http://p5trc.emv2.com/D2UTF8?emv_tag=4B6298341C808004&emv_ref=EdX7CqkmjRS88SA9MOPvvfTTWkp4FaXA_jmuc6k-WMDdK3E&EMAIL_FIELD={0}");

            _nmpServerAddress = GetSettingValue("NMPServerAddress",
                                                "http://api.notificationmessaging.com/nsapi/services/NotificationService");
            _nmpNotificationId = GetSettingValue("NMPNotificationId");
            _nmpEncrypt = GetSettingValue("NMPEncrypt");
            _nmpRandom = GetSettingValue("NMPRandom");
        }

        public override string FolderName
        {
            get { return FolderNames.SocialRibbonSetting; }
        }

        /// <summary>
        /// og:site_name
        /// </summary>
        /// <returns></returns>
        public string GetOpenGraphSiteName()
        {
            return GetSettingValue("og:site_name", "Enter the name of your website here");
        }

        /// <summary>
        /// fb:admins
        /// </summary>
        /// <returns></returns>
        public string GetFacebookAdmins()
        {
            return GetSettingValue("fb:admins", "Enter your facebook user ID here");
        }

        #region Share

        public string GetFacebookLink()
        {
            return GetSettingValue("FacebookLink");
        }

        public string GetFacebookCultureCode()
        {
            return GetSettingValue("FacebookCultureCode", "en_US");
        }

        public bool GetEnableFacebook()
        {
            return GetSettingValue("EnableFacebook", "true").AsBool();
        }

        public string GetTwitterLink()
        {
            return GetSettingValue("TwitterLink");
        }

        public string GetTwitterCultureCode()
        {
            return GetSettingValue("TwitterCultureCode", "en");
        }

        public bool GetEnableTwitter()
        {
            return GetSettingValue("EnableTwitter", "true").AsBool();
        }

        public string GetGooglePlusLink()
        {
            return GetSettingValue("GooglePlusLink");
        }

        public string GetGooglePlusCultureCode()
        {
            return GetSettingValue("GooglePlusCultureCode", "en-US");
        }

        public bool GetEnableGooglePlus()
        {
            return GetSettingValue("EnableGooglePlus", "true").AsBool();
        }

        public string GetYoutubeLink()
        {
            return GetSettingValue("YoutubeLink");
        }

        public bool GetEnableYoutube()
        {
            return GetSettingValue("EnableYoutube", "true").AsBool();
        }

        public string GetHyvesLink()
        {
            return GetSettingValue("HyvesLink");
        }

        public bool GetEnableHyves()
        {
            return GetSettingValue("EnableHyves", "true").AsBool();
        }

        public string GetWordPressLink()
        {
            return GetSettingValue("WordPressLink");
        }

        public bool GetEnableWordPress()
        {
            return GetSettingValue("EnableWordPress", "true").AsBool();
        }

        public string GetShareTitle(ViewDataDictionary viewData)
        {
            string shareTitle = viewData["ShareTitle"].AsString();
            if (String.IsNullOrEmpty(shareTitle))
            {
                shareTitle = GetShareTitle();
            }
            return shareTitle;
        }

        public string GetShareTitle()
        {
            return GetSettingValue("ShareTitle");
        }

        public string GetShareDescription(ViewDataDictionary viewData)
        {
            string shareDescription = viewData["ShareDescription"].AsString();
            if (string.IsNullOrEmpty(shareDescription))
            {
                shareDescription = GetShareDescription();
            }
            return shareDescription;
        }

        public string GetShareDescription()
        {
            return GetSettingValue("ShareDescription");
        }

        public string GetShareImage(ViewDataDictionary viewData)
        {
            string shareImage = viewData["ShareImage"].AsString();
            if (string.IsNullOrEmpty(shareImage))
            {
                shareImage = GetShareImage();
            }
            return shareImage;
        }

        public string GetShareImage()
        {
            return GetSettingValue("ShareImage");
        }

        public string GetShareUrl(ViewDataDictionary viewData)
        {
            string shareUrl = viewData["ShareUrl"].AsString();
            if (string.IsNullOrEmpty(shareUrl))
            {
                shareUrl = GetShareUrl();
            }
            return shareUrl;
        }

        public string GetShareUrl()
        {
            return GetSettingValue("ShareUrl");
        }

        #endregion

        private string _newsletterConnectionServerAddress;
        /// <summary>
        /// Newsletter Server address
        /// </summary>
        /// <returns></returns>
        public string GetNewsletterConnectionServerAddress()
        {
            return _newsletterConnectionServerAddress;
        }

        /// <summary>
        /// Newsletter API login
        /// </summary>
        /// <returns></returns>
        private string _newsletterConnectionLogin;
        public string GetNewsletterConnectionLogin()
        {
            return _newsletterConnectionLogin;
        }

        /// <summary>
        /// Newsletter API password
        /// </summary>
        /// <returns></returns>
        private string _newsletterConnectionPassword;
        public string GetNewsletterConnectionPassword()
        {
            return _newsletterConnectionPassword;
        }

        /// <summary>
        /// Newsletter API key
        /// </summary>
        /// <returns></returns>
        private string _newsletterConnectionKey;
        public string GetNewsletterConnectionKey()
        {
            return _newsletterConnectionKey;
        }

        /// <summary>
        /// Newsletter API LanguageID
        /// </summary>
        /// <returns></returns>
        private string _newsletterConnectionLanguageID;
        public string GetNewsletterConnectionLanguageID()
        {
            return _newsletterConnectionLanguageID;
        }

        private string _newsletterConnectionEmailStatus;
        /// <summary>
        /// Newsletter Email status
        /// </summary>
        /// <returns></returns>
        public string GetNewsletterConnectionEmailStatus()
        {
            return _newsletterConnectionEmailStatus;
        }

        private string _newsletterConnectionHttpCallAddress;
        /// <summary>
        /// HTTPcall to emailvision
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetNewsletterConnectionHttpCallAddress(string email)
        {
            return _newsletterConnectionHttpCallAddress.FormatWith(email);
        }

        private string _nmpServerAddress;
        public string GetNMPServerAddress()
        {
            return _nmpServerAddress;
        }

        private string _nmpNotificationId;
        public long GetNMPNotificationId()
        {
            long nmpNotificationId = 0;
            if (long.TryParse(_nmpNotificationId, out nmpNotificationId))
            {
                return nmpNotificationId;
            }
            return 0;
        }

        private string _nmpEncrypt;
        public string GetNMPEncrypt()
        {
            return _nmpEncrypt;
        }

        private string _nmpRandom;
        public string GetNMPRandom()
        {
            return _nmpRandom;
        }
    }
}