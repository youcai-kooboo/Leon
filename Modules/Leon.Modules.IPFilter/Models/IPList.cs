using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.IPFilter.Models
{
    public class IPList : TextContent
    {
        public IPList()
            : base(new TextContent())
        { }

        public IPList(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string IP = "IP";
            public const string Description = "Description";
        }

        public string IP
        {
            get
            {
                return this.GetString(FieldNames.IP);
            }
            set
            {
                this[FieldNames.IP] = value;
            }
        }
     
        public string Description
        {
            get
            {
                return this.GetString(FieldNames.Description);
            }
            set
            {
                this[FieldNames.Description] = value;
            }
        }
    }
}