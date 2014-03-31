using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Models
{
    public class RedirectBasedOnBrowserLanguage : TextContent
    {
        public RedirectBasedOnBrowserLanguage()
            : base(new TextContent())
        { }

        public RedirectBasedOnBrowserLanguage(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Site = "Site";
            public const string SubSite = "SubSite";
            public const string BrowserLanguage = "BrowserLanguage";
            public const string IsDefault = "IsDefault";
        }

        public string Site
        {
            get
            {
                return this.GetString(FieldNames.Site);
            }
            set
            {
                this[FieldNames.Site] = value;
            }
        }

        public string SubSite
        {
            get
            {
                return this.GetString(FieldNames.SubSite);
            }
            set
            {
                this[FieldNames.SubSite] = value;
            }
        }

        public string BrowserLanguage
        {
            get
            {
                return this.GetString(FieldNames.BrowserLanguage);
            }
            set
            {
                this[FieldNames.BrowserLanguage] = value;
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.GetBool(FieldNames.IsDefault);
            }
            set
            {
                this[FieldNames.IsDefault] = value;
            }
        }
    }
}