using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.IPFilter.Models
{
    public class IPSetting : TextContent
    {
        public IPSetting()
            : base(new TextContent())
        { }

        public IPSetting(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string FilterType = "FilterType";
            public const string FilterScope = "FilterScope";
            public const string ForbiddenHtml = "ForbiddenHtml";
        }

        public int FilterType
        {
            get
            {
                return this.GetInt(FieldNames.FilterType);
            }
            set
            {
                this[FieldNames.FilterType] = value;
            }
        }

        public int FilterScope
        {
            get
            {
                return this.GetInt(FieldNames.FilterScope);
            }
            set
            {
                this[FieldNames.FilterScope] = value;
            }
        }

        public string ForbiddenHtml
        {
            get
            {
                return this.GetString(FieldNames.ForbiddenHtml);
            }
            set
            {
                this[FieldNames.ForbiddenHtml] = value;
            }
        }
    }
}