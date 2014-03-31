using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Models
{
    public class PageMapping : TextContent
    {
        public PageMapping()
            : base(new TextContent())
        { }

        public PageMapping(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string RootSite = "RootSite";
            public const string FromSite = "FromSite";
            public const string FromPageUrl = "FromPageUrl";
            public const string ToSite = "ToSite";
            public const string ToPageUrl = "ToPageUrl";
        }

        /// <summary>
        /// Root site full name
        /// </summary>
        public string RootSite
        {
            get
            {
                return this.GetString(FieldNames.RootSite);
            }
            set
            {
                this[FieldNames.RootSite] = value;
            }
        }

        /// <summary>
        /// From site full name
        /// </summary>
        public string FromSite
        {
            get
            {
                return this.GetString(FieldNames.FromSite);
            }
            set
            {
                this[FieldNames.FromSite] = value;
            }
        }

        /// <summary>
        /// From page url
        /// </summary>
        public string FromPageUrl
        {
            get
            {
                return this.GetString(FieldNames.FromPageUrl);
            }
            set
            {
                this[FieldNames.FromPageUrl] = value;
            }
        }

        /// <summary>
        /// To site full name
        /// </summary>
        public string ToSite
        {
            get
            {
                return this.GetString(FieldNames.ToSite);
            }
            set
            {
                this[FieldNames.ToSite] = value;
            }
        }

        /// <summary>
        /// To page url
        /// </summary>
        public string ToPageUrl
        {
            get
            {
                return this.GetString(FieldNames.ToPageUrl);
            }
            set
            {
                this[FieldNames.ToPageUrl] = value;
            }
        }
    }
}