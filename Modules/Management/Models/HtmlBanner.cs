using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Models
{
    public class HtmlBanner : TextContent
    {
        public HtmlBanner()
            : base(new TextContent())
        { }

        public HtmlBanner(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Site = "Site";
            public const string Size = "Size";
            public const string Logo = "Logo";
            public const string Image = "Image";
            public const string Slogan = "Slogan";
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

        public string Size
        {
            get
            {
                return this.GetString(FieldNames.Size);
            }
            set
            {
                this[FieldNames.Size] = value;
            }
        }

        public string Logo
        {
            get
            {
                return this.GetString(FieldNames.Logo);
            }
            set
            {
                this[FieldNames.Logo] = value;
            }
        }

        public string Image
        {
            get
            {
                return this.GetString(FieldNames.Image);
            }
            set
            {
                this[FieldNames.Image] = value;
            }
        }

        public string Slogan
        {
            get
            {
                return this.GetString(FieldNames.Slogan);
            }
            set
            {
                this[FieldNames.Slogan] = value;
            }
        }
    }
}