using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Models
{
    public class Setting : TextContent
    {
        public Setting()
            : base(new TextContent())
        { }

        public Setting(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Site = "Site";
            public const string Key = "Key";
            public const string Value = "Value";
        }

        /// <summary>
        /// Site full name
        /// </summary>
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

        public string Key
        {
            get
            {
                return this.GetString(FieldNames.Key);
            }
            set
            {
                this[FieldNames.Key] = value;
            }
        }

        public string Value
        {
            get
            {
                return this.GetString(FieldNames.Value);
            }
            set
            {
                this[FieldNames.Value] = value;
            }
        }
    }
}