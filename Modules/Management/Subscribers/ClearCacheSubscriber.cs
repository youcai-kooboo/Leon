using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.EventBus;
using Kooboo.CMS.Content.EventBus.Content;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Subscribers
{
    public class ClearCacheSubscriber : ISubscriber
    {
        #region ISubscriber Members

        public EventResult Receive(IEventContext context)
        {
            if (context is ContentEventContext)
            {
                var contentEventContext = (ContentEventContext)context;
                if (contentEventContext.ContentAction == ContentAction.Add ||
                    contentEventContext.ContentAction == ContentAction.Delete ||
                    contentEventContext.ContentAction == ContentAction.Update)
                {
                    var content = contentEventContext.Content;

                    #region Setting

                    if (content.Repository.Equals("Management") && content.SchemaName.Equals(SchemaNames.Setting))
                    {
                        //ManagementContext.Current.SettingService.ClearSettingsCache();
                    }

                    #endregion

                    #region RedirectBasedOnBrowserLanguage

                    if (content.Repository.Equals("Management") && content.SchemaName.Equals(SchemaNames.RedirectBasedOnBrowserLanguage))
                    {
                        ManagementContext.Current.RedirectBasedOnBrowserLanguageService.ClearRedirectSettingsCache();
                    }

                    #endregion

                }
            }
            return null;
        }

        #endregion
    }
}