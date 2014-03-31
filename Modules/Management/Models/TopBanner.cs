using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Models
{
    public class TopBanner : TextContent
    {
             public TopBanner()
            : base(new TextContent())
        { }

             public TopBanner(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Title = "Title";
            public const string HtmlBanner = "HtmlBanner";
        }

        public string Title
        {
            get
            {
                return this.GetString(FieldNames.Title);
            }
            set
            {
                this[FieldNames.Title] = value;
            }
        }

        public string HtmlBanner
        {
            get
            {
                return this.GetString(FieldNames.HtmlBanner);
            }
            set
            {
                this[FieldNames.HtmlBanner] = value;
            }
        }
    }
}
